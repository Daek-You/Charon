using System.Collections.Generic;
using System.Diagnostics;

namespace CharacterController
{
    public class StateMachine
    {
        public BaseState CurrentState { get; private set; }
        private Dictionary<StateName, BaseState> states = new Dictionary<StateName, BaseState>();


        public StateMachine(StateName stateName, BaseState state)
        {
            AddState(stateName, state);
            CurrentState = GetState(stateName);
        }

        public void AddState(StateName stateName, BaseState state)
        {
            if (!states.ContainsKey(stateName))
            {
                states.Add(stateName, state);
            }
        }

        public BaseState GetState(StateName stateName)
        {
            if (states.TryGetValue(stateName, out BaseState state))
                return state;
            return null;
        }

        public void DeleteState(StateName removeStateName)
        {
            if (states.ContainsKey(removeStateName))
            {
                states.Remove(removeStateName);
            }
        }

        public void ChangeState(StateName nextStateName)
        {
            CurrentState?.OnExitState();
            if (states.TryGetValue(nextStateName, out BaseState newState))
            {
                CurrentState = newState;
            }
            CurrentState?.OnEnterState();
        }

        public void UpdateState()
        {
            CurrentState?.OnUpdateState();
        }

        public void FixedUpdateState()
        {
            CurrentState?.OnFixedUpdateState();
        }
    }
}

