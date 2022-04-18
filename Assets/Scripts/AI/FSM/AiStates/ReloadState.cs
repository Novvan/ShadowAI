using DeliverableIA.Core.Interfaces;


namespace DeliverableIA.AI.FSM.AiStates
{
	public class ReloadState<T> : State<T>
	{
		private INode _root;

		public ReloadState(StateMachine<T> fsm, INode root) : base(fsm)
		{
			_root = root;
		}
	}
}
