using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Gameplay;
using UnityEngine;

namespace SledSurfers.Scripts.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        private readonly Lazy<GameStateManager> _gameManager = new(ServiceLocator.Get<GameStateManager>);
        private GameStateManager GameStateManager => _gameManager.Value;
        
        public event Action<RunState> OnRunStateChanged;
        public RunState RunState { get; private set; }
        
        
        private void Awake()
        {
            GameStateManager.OnStateChanged += OnGameStateChanged;
            OnGameStateChanged(GameStateManager.GameState);
        }

        private void OnDestroy()
        {
            if (GameStateManager != null)
            {
                GameStateManager.OnStateChanged -= OnGameStateChanged;
            }
        }
        
        private void SwitchState(RunState newState)
        {
            RunState = newState;
            OnRunStateChanged?.Invoke(newState);
        }

        private void OnGameStateChanged(GameState newState)
        {
            if (newState == GameState.Playing)
            {
                SwitchState(RunState.Idle);
            }
            else
            {
                SwitchState(RunState.None);
            }
        }
    }
}