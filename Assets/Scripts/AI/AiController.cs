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
			Reload,
			Dead
		}

		private StateMachine<AIStates> _stateMachine;
		private INode _rootTreeNode;
		private Enemy _enemy;
		private float _counter;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			_enemy = GetComponent<Enemy>();
			InitializeStateMachine();
			InitializeDecisionTree();
		}

		private void Update()
		{
			_counter += Time.deltaTime;
			_rootTreeNode.Execute();
		}

		#endregion

		#region Custom Methods

		private void InitializeDecisionTree()
		{
			//Action Nodes
			var reloadActionNode = new ActionNode(() => { });
			var attackActionNode = new ActionNode(() => { });
			var chaseActionNode = new ActionNode(() => { });
			var patrolActionNode = new ActionNode(() => { });
			var idleActionNode = new ActionNode(() => { });

			//Question Nodes
			var isDoneIdling = new QuestionNode(IdleTimer, patrolActionNode, idleActionNode);
			var isInLineOfSight = new QuestionNode(() => _enemy.IsLineOfSight(), chaseActionNode, idleActionNode);
			var isAmmoEmpty = new QuestionNode(() => _enemy.IsAmmoEmpty(), attackActionNode, reloadActionNode);
			var isInAttackRange = new QuestionNode(() => _enemy.IsInAttackRange(), isAmmoEmpty, chaseActionNode);

			//Root Node
			_rootTreeNode = isDoneIdling;
		}

		private bool IdleTimer()
		{
			var cnt = _counter;
			_counter = 0;
			return cnt >= 5;
		}

		private void InitializeStateMachine()
		{
			_stateMachine = new StateMachine<AIStates>();
			var idleState = new IdleState<AIStates>(_stateMachine);
			var patrolState = new PatrolState<AIStates>(_stateMachine);
			// var chaseState = new ChaseState<AIStates>(_stateMachine);
			// var attackState = new AttackState<AIStates>(_stateMachine,2.5f,);
			var reloadState = new ReloadState<AIStates>(_stateMachine);
			var deadState = new DeadState<AIStates>(_stateMachine, 5f, gameObject);
		}

		#endregion
	}
}
