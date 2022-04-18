using DeliverableIA.Core.Interfaces;

namespace DeliverableIA.AI.FSM
{
	public class StateMachine<T>
	{
		private IState<T> _currentState;

		public StateMachine()
		{
		}

		public StateMachine(IState<T> initialState)
		{
			SetInit(initialState);
		}

		public void SetInit(IState<T> initialState)
		{
			_currentState = initialState;
			_currentState.Enter();
		}

		public void OnUpdate()
		{
			_currentState?.Execute();
		}

		public void Transition(T input)
		{
			var newState = _currentState.GetTransition(input);
			if (newState == null) return;
			_currentState.Exit();
			SetInit(newState);
		}
	}
}
