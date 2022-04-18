using System;
using DeliverableIA.Core.Interfaces;
using UnityEngine;

namespace DeliverableIA.AI.Trees
{
	public class QuestionNode : INode
	{
		#region Variables

		private readonly Func<bool> _question;
		private readonly INode _trueNode, _falseNode;

		#endregion

		public QuestionNode(Func<bool> question, INode nTrue, INode nFalse)
		{
			_question = question;
			_trueNode = nTrue;
			_falseNode = nFalse;
		}

		public void Execute()
		{
			if (_question())
			{
				_trueNode.Execute();
			}
			else
			{
				_falseNode.Execute();
			}
		}
	}
}
