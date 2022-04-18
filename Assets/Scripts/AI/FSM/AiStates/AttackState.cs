using DeliverableIA.Core.Interfaces;

namespace DeliverableIA.AI.FSM.AiStates
{
	public class AttackState<T> : State<T>
	{
		public AttackState(StateMachine<T> fsm, float rateOfFire) : base(fsm)
		{
		}
	}
}
