using DeliverableIA.Core.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace DeliverableIA.AI.FSM.AiStates
{
	public class PatrolState<T> : State<T>
	{
		#region Variables

		private readonly NavMeshAgent _agent;
		private readonly Transform[] _waypoints;
		private int _waypointIndex = 0;
		private Vector3 _targetWaypoint;
		private readonly Enemy _enemy;

		#endregion

		public PatrolState(INode root, Enemy enemy) : base(enemy, root)
		{
			_enemy = enemy;
			_waypoints = _enemy.waypoints;
			_agent = _enemy.MeshAgent;
		}

		#region Class Methods

		public override void Enter()
		{
			UpdateDestination();
			IterateWaypointIndex();
		}

		public override void Execute()
		{
			var targets = EnemyEntity.CheckTargets(EnemyEntity.sightRange);
			if (targets != null && targets.Length > 0)
			{
				Root.Execute();
			}

			if (Vector3.Distance(_agent.transform.position, _targetWaypoint) < 1f) Root.Execute();
		}

		public override void Exit()
		{
			_enemy.ResetNav();
		}

		#endregion


		#region Custom Methods

		private void UpdateDestination()
		{
			_targetWaypoint = _waypoints[_waypointIndex].position;
			_agent.transform.LookAt(_targetWaypoint);
			_agent.SetDestination(_targetWaypoint);
		}

		private void IterateWaypointIndex()
		{
			_waypointIndex++;
			if (_waypointIndex == _waypoints.Length)
				_waypointIndex = 0;
		}

		#endregion
	}
}
