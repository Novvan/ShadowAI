using DeliverableIA.Core.Interfaces;
using UnityEngine;

namespace DeliverableIA.AI.FSM.AiStates
{
	public class ChaseState<T> : State<T>
	{
		#region Variables

		private readonly GameObject _player;

		#endregion

		public ChaseState(INode root, Enemy enemy, GameObject player) : base(enemy, root)
		{
			_player = player;
		}


		#region Class Methods

		public override void Execute()
		{
			base.Execute();
			EnemyEntity.Chase(_player);

			var targetsOnSight = EnemyEntity.CheckTargets(EnemyEntity.sightRange);
			if (targetsOnSight == null || targetsOnSight.Length == 0)
			{
				Root.Execute();
			}
			else
			{
				var targetsOnRange = EnemyEntity.CheckTargets(EnemyEntity.Weapon.range);
				if (targetsOnRange == null || targetsOnRange.Length <= 0) return;
				Root.Execute();
			}
		}

		public override void Exit()
		{
			base.Exit();
			EnemyEntity.ResetNav();
		}

		#endregion
	}
}
