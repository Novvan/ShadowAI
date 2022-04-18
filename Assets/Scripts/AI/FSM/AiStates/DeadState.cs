using UnityEngine;

namespace DeliverableIA.AI.FSM.AiStates
{
	public class DeadState<T> : State<T>
	{
		#region Variables

		private readonly float _deathTimer;
		private float _counter;
		private readonly GameObject _entity;

		#endregion

		public DeadState(StateMachine<T> fsm, float deathTimer, GameObject entity) : base(fsm)
		{
			_deathTimer = deathTimer;
			_entity = entity;
		}

		public override void Enter()
		{
			_counter = _deathTimer;
		}

		public override void Execute()
		{
			base.Execute();
			_counter -= Time.deltaTime;

			if (_counter <= 0)
			{
				Object.Destroy(_entity);
			}
		}
	}
}
