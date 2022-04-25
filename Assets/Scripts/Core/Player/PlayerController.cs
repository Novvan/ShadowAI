using UnityEngine;
using Cinemachine;
using DeliverableIA.Core.ScriptableObjects;
using DeliverableIA.Core.Utils;
using DeliverableIA.Imported_Assets.StarterAssets.InputSystem;
using DeliverableIA.Imported_Assets.StarterAssets.ThirdPersonController.Scripts;

namespace DeliverableIA.Core.Player
{
	[RequireComponent(typeof(Player))]
	public class PlayerController : MonoBehaviour
	{
		#region Variables

		private StarterAssetsInputs _starterAssetsInputs;
		private ThirdPersonController _thirdPersonController;
		private Animator _animator;
		private Player _player;

		[Header("Camera")]
		[SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
		[SerializeField] private float baseSensitivity = 1f, aimSensitivity = 0.5f;
		[SerializeField] private LayerMask aimCollisionMask = new LayerMask();
		
		#endregion

		#region Unity Methods

		private void Awake()
		{
			_starterAssetsInputs = GetComponent<StarterAssetsInputs>();
			_thirdPersonController = GetComponent<ThirdPersonController>();
			_animator = GetComponent<Animator>();
			_player = GetComponent<Player>();
		}

		private void Update()
		{
			HandleAim();
		}

		#endregion

		#region Custom Methods

		private void HandleAim()
		{
			if (_starterAssetsInputs.aim)
			{
				aimVirtualCamera.gameObject.SetActive(true);
				_thirdPersonController.SetCameraSensitivity(aimSensitivity);
				_thirdPersonController.SetRotateOnMove(false);

				
				var mouseWorldPosition = Vector3.zero;
				var screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
				var ray = Tools.Cam.ScreenPointToRay(screenCenterPoint);
				if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimCollisionMask))
				{
					mouseWorldPosition = hit.point;
				}

				var worldAimTarget = mouseWorldPosition;
				var pos = transform.position;
				worldAimTarget.y = pos.y;
				var aimDirection = (worldAimTarget - pos).normalized;

				_animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1, Time.deltaTime * 10f));

				transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);


				if (_starterAssetsInputs.shoot)
				{
					_player.Shoot(mouseWorldPosition);
					_starterAssetsInputs.shoot = false;

				}
			}
			else
			{
				aimVirtualCamera.gameObject.SetActive(false);
				_animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 0, Time.deltaTime * 10f));
				_thirdPersonController.SetCameraSensitivity(baseSensitivity);
				_thirdPersonController.SetRotateOnMove(true);
			}
		}

		#endregion
	}
}
