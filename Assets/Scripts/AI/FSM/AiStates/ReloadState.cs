using DeliverableIA.Core.Interfaces;
using UnityEngine;


namespace DeliverableIA.AI.FSM.AiStates
{
	public class ReloadState<T> : State<T>
	{
		#region Variables

		private float _counter = 0f;

		#endregion

		public ReloadState(INode root, Enemy enemy) : base(enemy, root)
		{
		}


		#region Class Methods

		public override void Enter()
		{
			base.Enter();
			EnemyEntity.ResetNav();
		}

		public override void Execute()
		{
			base.Execute();
			_counter += Time.deltaTime;
			if (!(_counter >= EnemyEntity.Weapon.reloadTime)) return;
			EnemyEntity.Reload();
			Root.Execute();
		}

		public override void Exit()
		{
			base.Exit();
			_counter = 0f;
		}

		#endregion
	}
}
