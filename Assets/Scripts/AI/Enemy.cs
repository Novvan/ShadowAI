using DeliverableIA.Core;
using DeliverableIA.Core.Player;
using DeliverableIA.Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace DeliverableIA.AI
{
	[SelectionBase]
	[RequireComponent(typeof(NavMeshAgent))]
	public class Enemy : Entity
	{
		#region Variables

		[Header("Sight")]
		public float sightAngle = 10f;
		public float sightRange = 10f;
		public LayerMask obstacleMask = new LayerMask(), playerMask = new LayerMask();
		
		[Space(5)]
		[Header("State")]
		public float idleTime;
		public Transform[] waypoints;

		private NavMeshAgent _navMeshAgent;

		public NavMeshAgent MeshAgent => _navMeshAgent;

		public Weapon Weapon => weapon;
		public int CurrentAmmunition => CurrentAmmo;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			_navMeshAgent = GetComponent<NavMeshAgent>();
		}

		#endregion

		#region Custom Methods

		public Transform[] CheckTargets(float range)
		{
			var colliders = new Collider[10];
			var numColliders = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, playerMask);
			var targets = new Transform[numColliders];
			for (var i = 0; i < numColliders; i++)
			{
				targets[i] = colliders[i].transform;
			}

			return targets;
		}


		public bool IsInRange(Transform target, float range)
		{
			//Check if target is in range
			var diff = target.position - transform.position;
			var distance = diff.magnitude;
			if (distance > range) return false;

			//Check if target is inside angle of view
			var angleToTarget = Vector3.Angle(diff, transform.forward);
			if (angleToTarget > sightAngle / 2) return false;

			//Check if target is visible
			return !Physics.Raycast(transform.position, diff.normalized, distance, obstacleMask);
		}

		public void Reload()
		{
			if (TotalAmmo <= 0) return;
			CurrentAmmo = weapon.magazineSize;
			TotalAmmo -= weapon.magazineSize;
		}

		public void Chase(GameObject player)
		{
			_navMeshAgent.SetDestination(player.transform.position);
		}

		public void ResetNav()
		{
			_navMeshAgent.isStopped = true;
			_navMeshAgent.ResetPath();
			_navMeshAgent.isStopped = false;
		}

		#endregion
	}
}
