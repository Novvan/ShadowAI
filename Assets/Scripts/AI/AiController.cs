using DeliverableIA.AI.FSM;
using DeliverableIA.AI.FSM.AiStates;
using DeliverableIA.AI.Trees;
using DeliverableIA.Core.Interfaces;
using UnityEngine;

namespace DeliverableIA.AI
{
    [RequireComponent(typeof(Enemy))]
    public class AiController : MonoBehaviour
    {
        #region Variables

        private enum AIStates
        {
            Idle,
            Patrol,
            Chase,
            Attack,
            Reload
        }

        private StateMachine<AIStates> _stateMachine;
        private INode _rootTreeNode;
        private Enemy _enemy;
        private float _rateOfFire;
        private IdleState<AIStates> _idle;
        private int _cFrameIsLoS, _cFrameIsAtkRange;
        private bool _cIsLoS, _cIsAtkRange;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();

            _rateOfFire = _enemy.Weapon.fireRate;

            InitializeStateMachine();
            InitializeDecisionTree();
        }

        private void Update()
        {
            _stateMachine.OnUpdate();
        }

        #endregion

        #region Custom Methods

        private void InitializeDecisionTree()
        {
            //Action Nodes
            var transitionToReload = new ActionNode(() => _stateMachine.Transition(AIStates.Reload));
            var transitionToAttack = new ActionNode(() => _stateMachine.Transition(AIStates.Attack));
            var transitionToChase = new ActionNode(() => _stateMachine.Transition(AIStates.Chase));
            var transitionToPatrol = new ActionNode(() => _stateMachine.Transition(AIStates.Patrol));
            var transitionToIdle = new ActionNode(() => _stateMachine.Transition(AIStates.Idle));

            //Question Nodes
            var isAmmoEmpty = new QuestionNode(() => IsAmmoEmpty(), transitionToAttack, transitionToReload);
            var isInAttackRange = new QuestionNode(() => IsInAttackRange(), isAmmoEmpty, transitionToChase);
            var isInLineOfSight = new QuestionNode(() => IsLineOfSight(), isInAttackRange, transitionToPatrol);
            var isDoneIdling = new QuestionNode(() => _idle.IdleTimer(), isInLineOfSight, transitionToIdle);

            //Root Node
            _rootTreeNode = isDoneIdling;
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine<AIStates>();
            var idleState = new IdleState<AIStates>(_stateMachine, _rootTreeNode);
            var patrolState = new PatrolState<AIStates>(_stateMachine, _enemy.waypoints, _enemy.MeshAgent);
            var chaseState = new ChaseState<AIStates>(_stateMachine);
            var attackState = new AttackState<AIStates>(_stateMachine, _rateOfFire);
            var reloadState = new ReloadState<AIStates>(_stateMachine);

            idleState.AddTransition(AIStates.Patrol, patrolState);
            idleState.AddTransition(AIStates.Chase, chaseState);

            patrolState.AddTransition(AIStates.Idle, idleState);
            patrolState.AddTransition(AIStates.Chase, chaseState);

            chaseState.AddTransition(AIStates.Idle, idleState);
            chaseState.AddTransition(AIStates.Patrol, patrolState);
            chaseState.AddTransition(AIStates.Attack, attackState);
            chaseState.AddTransition(AIStates.Reload, reloadState);

            attackState.AddTransition(AIStates.Reload, reloadState);
            attackState.AddTransition(AIStates.Chase, chaseState);

            reloadState.AddTransition(AIStates.Attack, attackState);
            reloadState.AddTransition(AIStates.Chase, chaseState);

            _idle = idleState;
			
            _stateMachine.SetInit(idleState);
        }

        public bool IsAmmoEmpty() => _enemy.CurrentAmmo > 0;
        public bool IsLineOfSight()
        {
            if (_cFrameIsLoS == Time.frameCount)
                return _cIsLoS;
            else
            {
                _cFrameIsLoS = Time.frameCount;
                //TODO: IMPLEMENT LINE OF SIGHT
                return _cIsLoS;
            }
        }
        public bool IsInAttackRange()
        {
            if (_cFrameIsAtkRange == Time.frameCount)
                return _cIsAtkRange;
            else
            {
                _cFrameIsAtkRange = Time.frameCount;
                //TODO: IMPLEMENT ATTACK RANGE
                return _cIsAtkRange;
            }
        }

        #endregion
    }
}
