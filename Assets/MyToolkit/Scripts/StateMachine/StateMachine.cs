
using System;
using System.Collections.Generic;
using System.Linq;

namespace MTK.StateMachine
{
    public class StateMachine
    {
        private Dictionary<Type, State> _states;
        private State _currentState;

        public StateMachine(Dictionary<Type, State> states)
        {
            _states = states;

            foreach (var pair in states)
                pair.Value.Init(this);

            _currentState = _states.First().Value;
            _currentState.OnEnter();
        }

        public void Enter<T>() where T : State
        {
            _currentState.OnExit();
            _currentState = _states[typeof(T)];
            _currentState.OnEnter();
        }

        public void Update()
        {
            _currentState.OnUpdate();
        }
    };
}