using System;
using UnityEngine;
using Cinemachine;
using DeliverableIA.Core.Utils;
using DeliverableIA.Imported_Assets.StarterAssets.InputSystem;
using DeliverableIA.Imported_Assets.StarterAssets.ThirdPersonController.Scripts;

namespace DeliverableIA.Core.Player
{
	public class PlayerController : MonoBehaviour
	{
		#region Variables

		private StarterAssetsInputs _starterAssetsInputs;
		private ThirdPersonController _thirdPersonController;
		private Animator _animator;

		[Header("Camera")]
		[SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
		[SerializeField] private float baseSensitivity = 1f, aimSensitivity = 0.5f;
		[SerializeField] private LayerMask aimCollisionMask = new LayerMask();

		[Header("Shooting")]
		[SerializeField] private Transform pfGun;
		[SerializeField] private Transform gunSocket;
		[SerializeField] private Transform pfBullet;
		private Transform spawnPoint;
		

		#endregion

		#region Unity Methods

		private void Awake()
		{
			_starterAssetsInputs = GetComponent<StarterAssetsInputs>();
			_thirdPersonController = GetComponent<ThirdPersonController>();
			_animator = GetComponent<Animator>();
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
				
				var gun = Instantiate(pfGun, gunSocket);

				spawnPoint = gun.GetChild(3);

				Vector3 mouseWorldPosition = Vector3.zero;
				Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
				Ray ray = Tools.Cam.ScreenPointToRay(screenCenterPoint);
				if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimCollisionMask))
				{
					mouseWorldPosition = hit.point;
				}

				Vector3 worldAimTarget = mouseWorldPosition;
				worldAimTarget.y = transform.position.y;
				Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
				
				_animator.SetLayerWeight(1,Mathf.Lerp(_animator.GetLayerWeight(1),1,Time.deltaTime * 10f));
				
				transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);


				if (_starterAssetsInputs.shoot)
				{
					Vector3 aimDir = (mouseWorldPosition - spawnPoint.position).normalized;
					Instantiate(pfBullet, spawnPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
					_starterAssetsInputs.shoot = false;
				}
			}
			else
			{
				aimVirtualCamera.gameObject.SetActive(false);
				_animator.SetLayerWeight(1,Mathf.Lerp(_animator.GetLayerWeight(1),0,Time.deltaTime * 10f));
				_thirdPersonController.SetCameraSensitivity(baseSensitivity);
				_thirdPersonController.SetRotateOnMove(true);
				gunSocket.DeleteAllChildren();
			}
		}

		#endregion
	}
}
