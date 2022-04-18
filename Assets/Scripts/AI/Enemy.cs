using DeliverableIA.Core.ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace DeliverableIA.AI
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class Enemy : MonoBehaviour
	{
		#region Variables

		[SerializeField] private float speed = 1f;
		[SerializeField] private int maxAmmo = 10;
		[SerializeField] private Weapon weapon;
		public Transform[] waypoints;
		private int _currentAmmo;
		private float _counter;
		private NavMeshAgent _navMeshAgent;

		public Weapon Weapon => weapon;

		public NavMeshAgent MeshAgent => _navMeshAgent;

		#endregion

		#region Unity Methods

		private void Start()
		{
			_currentAmmo = maxAmmo;
			_navMeshAgent = GetComponent<NavMeshAgent>();
		}

		private void Update()
		{
			_counter += Time.deltaTime;
		}

		#endregion

		#region Custom Methods

		public bool IsAmmoEmpty() => _currentAmmo > 0;
		public bool IsLineOfSight() => true;
		public bool IsInAttackRange() => false;

		public bool IdleTimer()
		{
			var cnt = _counter;
			_counter = 0;
			return cnt >= 5;
		}

		#endregion
	}
}
