using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Gameplay;
using UnityEngine;

namespace SledSurfers.Scripts.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        private readonly Lazy<GameManager> _gameManager = new(ServiceLocator.Get<GameManager>);
        private GameManager GameManager => _gameManager.Value;
        
        public event Action<RunState> OnRunStateChanged;
        public RunState RunState { get; private set; }
        
        
        private void Awake()
        {
            GameManager.OnStateChanged += OnGameStateChanged;
            OnGameStateChanged(GameManager.GameState);
        }

        private void OnDestroy()
        {
            if (GameManager != null)
            {
                GameManager.OnStateChanged -= OnGameStateChanged;
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