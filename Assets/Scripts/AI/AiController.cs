using System;
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

		#endregion

		#region Unity Methods

		private void Awake()
		{
			_enemy = GetComponent<Enemy>();
			
			_rateOfFire = _enemy.Weapon.fireRate;
			
			InitializeStateMachine();
			InitializeDecisionTree();
		}

		private void Start()
		{
			_rootTreeNode.Execute();
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
			var isDoneIdling = new QuestionNode(() => _enemy.IdleTimer(), transitionToPatrol, transitionToIdle);
			var isInLineOfSight = new QuestionNode(() => _enemy.IsLineOfSight(), transitionToChase, transitionToIdle);
			var isAmmoEmpty = new QuestionNode(() => _enemy.IsAmmoEmpty(), transitionToAttack, transitionToReload);
			var isInAttackRange = new QuestionNode(() => _enemy.IsInAttackRange(), isAmmoEmpty, transitionToChase);

			//Root Node
			_rootTreeNode = isDoneIdling;
		}

		private void InitializeStateMachine()
		{
			_stateMachine = new StateMachine<AIStates>();
			var idleState = new IdleState<AIStates>(_stateMachine);
			var patrolState = new PatrolState<AIStates>(_stateMachine);
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


			_stateMachine.SetInit(idleState);
		}

		#endregion
	}
}
