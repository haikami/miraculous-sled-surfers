using System;
using SledSurfers.Scripts.Core;

namespace SledSurfers.Scripts.Managers
{
    public class GameManager : IInitializable, IDisposable
    {
        public event Action<GameState> OnStateChanged;
        public GameState GameState { get; private set; } = GameState.Loading;

        public void Initialize()
        {
            if (ServiceLocator.TryGet(out LevelManager manager))
            {
                manager.OnLevelLoaded += OnLevelLoaded;
            }
        }

        public void StartGame()
        {
            SwitchState(GameState.Playing);
        }

        private void OnLevelLoaded()
        {
            SwitchState(GameState.MainMenu);
        }

        private void SwitchState(GameState newState)
        {
            GameState = newState;
            OnStateChanged?.Invoke(newState);
        }
        
        public void Dispose()
        {
            if (ServiceLocator.TryGet(out LevelManager manager))
            {
                manager.OnLevelLoaded -= OnLevelLoaded;
            }
        }
    }
}
