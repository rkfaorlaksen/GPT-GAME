using System;

namespace MilkyWayStation.UI.StateMachine
{
    public sealed class UiStateMachine
    {
        public GameUiState CurrentState { get; private set; } = GameUiState.Garden;
        public event Action<GameUiState> StateChanged;

        public void Transition(GameUiState next)
        {
            if (CurrentState == next) return;
            CurrentState = next;
            StateChanged?.Invoke(CurrentState);
        }
    }
}
