using DeliverableIA.Core.Interfaces;
using UnityEngine;

namespace DeliverableIA.AI.FSM.AiStates
{
	public class IdleState<T> : State<T>
	{
		#region Variables

		private float _counter;

		#endregion

		public IdleState(INode root, Enemy enemy) : base(enemy, root)
		{
		}

		#region Class Methods

		public override void Execute()
		{
			base.Execute();
			var targets = EnemyEntity.CheckTargets(EnemyEntity.sightRange);
			if (targets != null && targets.Length > 0)
			{
				Root.Execute();
			}
			_counter += Time.deltaTime;
			if (IdleTimer()) Root.Execute();
		}

		public override void Exit()
		{
			base.Exit();
			_counter = 0;
		}

		#endregion

		#region Utils

		public bool IdleTimer() => _counter >= EnemyEntity.idleTime;

		#endregion
	}
}
