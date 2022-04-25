using System.Collections.Generic;
using System.Linq;
using DeliverableIA.Core.Interfaces;

namespace DeliverableIA.AI.FSM
{
	public class State<T> : IState<T>
	{
		#region Variables

		private readonly Dictionary<T, IState<T>> _transitions = new Dictionary<T, IState<T>>();
		protected readonly Enemy EnemyEntity;
		protected readonly INode Root;

		#endregion

		protected State(Enemy enemy, INode root)
		{
			EnemyEntity = enemy;
			Root = root;
		}

		#region Class Methods

		public virtual void Enter()
		{
		}

		public virtual void Execute()
		{
		}

		public virtual void Exit()
		{
		}

		#endregion

		#region Utils

		public void AddTransition(T key, IState<T> state)
		{
			_transitions[key] = state;
		}

		public void DeleteTransition(IState<T> state)
		{
			foreach (var transition in _transitions.Where(transition => transition.Value == state))
			{
				_transitions.Remove(transition.Key);
			}
		}

		public IState<T> GetTransition(T key)
		{
			var transition = _transitions.FirstOrDefault(t => t.Key.Equals(key));
			return transition.Value;
		}

		#endregion
	}
}
