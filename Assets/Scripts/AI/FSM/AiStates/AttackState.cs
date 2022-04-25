using DeliverableIA.Core.Interfaces;
using UnityEngine;

namespace DeliverableIA.AI.FSM.AiStates
{
	public class AttackState<T> : State<T>
	{
		#region Variables

		private readonly GameObject _player;
		private float _counter = 0f;

		#endregion

		public AttackState(INode root, Enemy enemy, GameObject player) : base(enemy, root)
		{
			_player = player;
		}

		#region Class Methods

		public override void Enter()
		{
			base.Enter();
			EnemyEntity.MeshAgent.isStopped = true;
		}

		public override void Execute()
		{
			base.Execute();
			EnemyEntity.Chase(_player);

			var targetsOnRange = EnemyEntity.CheckTargets(EnemyEntity.Weapon.range);
			if (targetsOnRange == null || targetsOnRange.Length == 0)
			{
				Root.Execute();
			}
			else
			{
				if (_counter <= 0)
				{
					_counter = EnemyEntity.Weapon.fireRate;
					EnemyEntity.Shoot(_player.transform.position);
					Root.Execute();
				}
				else
				{
					_counter -= Time.deltaTime;
				}
			}
		}

		public override void Exit()
		{
			base.Exit();
			EnemyEntity.MeshAgent.isStopped = false;
		}

		#endregion
	}
}
