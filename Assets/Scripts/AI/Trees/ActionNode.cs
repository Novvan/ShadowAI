using System;
using DeliverableIA.Core.Interfaces;

namespace DeliverableIA.AI.Trees
{
	public class ActionNode : INode
	{
		#region Variables

		private readonly Action _action;

		#endregion

		public ActionNode(Action action)
		{
			_action = action;
		}

		public void Execute()
		{
			_action();
		}
	}
}
