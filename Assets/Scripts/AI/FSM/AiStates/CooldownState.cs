using DeliverableIA.Core.Interfaces;
using UnityEngine;

namespace DeliverableIA.AI.FSM.AiStates
{
	public class CooldownState<T> : State<T>
	{
		#region Variables

		private readonly float _time;
		private readonly INode _root;
		private float _counter;

		#endregion

		protected CooldownState(StateMachine<T> fsm, float time, INode root) : base(fsm)
		{
			_root = root;
			_time = time;
		}

		public override void Enter()
		{
			_counter = _time;
		}

		public override void Execute()
		{
			_counter -= Time.deltaTime;
			if (!(_counter <= 0)) return;

			_root.Execute();
			_counter = _time;
		}
	}
}
