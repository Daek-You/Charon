using System.Collections.Generic;

namespace CharacterController
{
    public class StateMachine
    {
        public BaseState currentState { get; private set; }
        private Dictionary<StateName, BaseState> states = new Dictionary<StateName, BaseState>();

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
            currentState?.OnExitState();
            if (states.TryGetValue(nextStateName, out BaseState newState))
            {
                currentState = newState;
            }
            currentState?.OnEnterState();
        }

        public void UpdateState()
        {
            currentState?.OnUpdateState();
        }

        public void FixedUpdateState()
        {
            currentState?.OnFixedUpdateState();
        }
    }
}

