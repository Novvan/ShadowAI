using DeliverableIA.Core.Interfaces;
using UnityEngine;

namespace DeliverableIA.AI.FSM.AiStates
{
    public class IdleState<T> : State<T>
    {
        private float _counter;
        private INode _root;

        public IdleState(StateMachine<T> fsm, INode root) : base(fsm)
        {
            _root = root;
        }

        public override void Execute()
        {
            base.Execute();

            _counter += Time.deltaTime;
            if (IdleTimer()) _root.Execute();
        }

        public override void Exit()
        {
            base.Exit();
            _counter = 0;
        }

        public bool IdleTimer()
        {
            var cnt = _counter;
            _counter = 0;
            return cnt >= 5;
        }
    }
}
