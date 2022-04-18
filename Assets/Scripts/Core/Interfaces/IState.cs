namespace DeliverableIA.Core.Interfaces
{
	public interface IState<T>
	{
		void Enter();
		void Execute();
		void Exit();
		public void AddTransition(T key, IState<T> state);
		public void DeleteTransition(IState<T> state);
		public IState<T> GetTransition(T key);
	}
}
