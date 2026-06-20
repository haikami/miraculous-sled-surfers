using System;

namespace SledSurfers.Scripts.Managers
{
    public class StateManager<T> where T : Enum
    {
        public event Action<T> OnStateChanged;

        public T CurrentState { get; private set; }

        public StateManager()
        {
            CurrentState = default(T);
        }

        public StateManager(T initialState)
        {
            CurrentState = initialState;
        }

        public void SwitchState(T newState)
        {
            if (Equals(CurrentState, newState))
                return;

            CurrentState = newState;
            OnStateChanged?.Invoke(newState);
        }
    }
}