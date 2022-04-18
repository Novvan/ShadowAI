using DeliverableIA.Core.Interfaces;

namespace DeliverableIA.AI.FSM.AiStates
{
	public class AttackState<T> : CooldownState<T>
	{
		protected AttackState(StateMachine<T> fsm, float time, INode root) : base(fsm, time, root)
		{
		}
	}
}
