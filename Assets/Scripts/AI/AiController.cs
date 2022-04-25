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
		private IdleState<AIStates> _idle;
		private INode _rootTreeNode;
		private Enemy _enemy;
		private GameObject _player;
		private float _rateOfFire, _sightRange, _attackRange;
		private int _cFrameIsLoS, _cFrameIsAtkRange;
		private bool _cIsLoS, _cIsAtkRange;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			_enemy = GetComponent<Enemy>();

			_rateOfFire = _enemy.Weapon.fireRate;
			_sightRange = _enemy.sightRange;
			_attackRange = _enemy.Weapon.range;
		}

		private void Start()
		{
			_player = GameObject.FindGameObjectWithTag("Player");
			InitializeDecisionTree();
			InitializeStateMachine();
		}

		private void Update()
		{
			_stateMachine.OnUpdate();
			// Debug.Log("Current State: " + _stateMachine.GetCurrentState().GetType().Name);
		}

		#endregion

		#region Custom Methods

		private void InitializeDecisionTree()
		{
			//Action Nodes
			var transitionToIdle = new ActionNode(() => { _stateMachine.Transition(AIStates.Idle); });
			var transitionToPatrol = new ActionNode(() => { _stateMachine.Transition(AIStates.Patrol); });
			var transitionToChase = new ActionNode(() => { _stateMachine.Transition(AIStates.Chase); });
			var transitionToAttack = new ActionNode(() => { _stateMachine.Transition(AIStates.Attack); });
			var transitionToReload = new ActionNode(() => { _stateMachine.Transition(AIStates.Reload); });

			//Question Nodes
			var isAmmoEmpty = new QuestionNode(IsAmmoEmpty, transitionToReload, transitionToAttack);
			var isInAttackRange = new QuestionNode(IsInAttackRange, isAmmoEmpty, transitionToChase);
			var isDoneIdling = new QuestionNode(() => _idle.IdleTimer(), transitionToPatrol, transitionToIdle);
			var isInLineOfSight = new QuestionNode(IsLineOfSight, isInAttackRange, isDoneIdling);

			//Root Node
			_rootTreeNode = isInLineOfSight;
		}

		private void InitializeStateMachine()
		{
			_stateMachine = new StateMachine<AIStates>();
			var idleState = new IdleState<AIStates>(_rootTreeNode, _enemy);
			var patrolState = new PatrolState<AIStates>(_rootTreeNode, _enemy);
			var chaseState = new ChaseState<AIStates>(_rootTreeNode, _enemy, _player);
			var attackState = new AttackState<AIStates>(_rootTreeNode, _enemy, _player);
			var reloadState = new ReloadState<AIStates>(_rootTreeNode, _enemy);

			idleState.AddTransition(AIStates.Patrol, patrolState);
			idleState.AddTransition(AIStates.Attack, attackState);
			idleState.AddTransition(AIStates.Chase, chaseState);

			patrolState.AddTransition(AIStates.Idle, idleState);
			patrolState.AddTransition(AIStates.Chase, chaseState);
			patrolState.AddTransition(AIStates.Attack, attackState);
			patrolState.AddTransition(AIStates.Reload, reloadState);

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

		private bool IsAmmoEmpty() => _enemy.CurrentAmmunition <= 0;

		private bool IsLineOfSight()
		{
			if (_cFrameIsLoS == Time.frameCount)
				return _cIsLoS;
			else
			{
				var targets = _enemy.CheckTargets(_sightRange);
				for (var i = 0; i < targets.Length; i++)
				{
					if (_enemy.IsInRange(_player.transform, _sightRange))
					{
						_cIsLoS = true;
						break;
					}

					_cIsLoS = false;
				}

				_cFrameIsLoS = Time.frameCount;
				return _cIsLoS;
			}
		}

		private bool IsInAttackRange()
		{
			if (_cFrameIsAtkRange == Time.frameCount)
				return _cIsAtkRange;
			else
			{
				var targets = _enemy.CheckTargets(_attackRange);
				for (var i = 0; i < targets.Length; i++)
				{
					if (_enemy.IsInRange(_player.transform, _attackRange))
					{
						_cIsAtkRange = true;
						break;
					}

					_cIsAtkRange = false;
				}
				_cFrameIsAtkRange = Time.frameCount;
				return _cIsAtkRange;
			}
		}

		#endregion
	}
}
