using UnityEngine;
using UnityEngine.AI;

namespace DeliverableIA.AI.FSM.AiStates
{
    public class PatrolState<T> : State<T>
    {
        private readonly NavMeshAgent _agent;
        private readonly Transform[] _waypoints;
        private int _waypointIndex;
        private Vector3 _targetWaypoint;

        public PatrolState(StateMachine<T> fsm, Transform[] patrolWaypoints, NavMeshAgent agent) : base(fsm)
        {
            _waypoints = patrolWaypoints;
            _agent = agent;
        }

        public override void Enter()
        {
            UpdateDestination();
        }


        private void UpdateDestination()
        {
            _targetWaypoint = _waypoints[_waypointIndex].position;
            _agent.SetDestination(_targetWaypoint);
        }

        private void IterateWaypointIndex()
        {
            _waypointIndex++;
            if (_waypointIndex == _waypoints.Length)
                _waypointIndex = 0;
        }
    }
}
