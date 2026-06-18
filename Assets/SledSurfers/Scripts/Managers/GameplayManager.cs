using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Gameplay.Slingshot;
using SledSurfers.Scripts.Player;
using UnityEngine;

namespace SledSurfers.Scripts.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private SlingshotManager _slingshotManager;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CameraController _cameraController;
        
        
        private readonly Lazy<GameStateManager> _gameManager = new(ServiceLocator.Get<GameStateManager>);
        private GameStateManager GameStateManager => _gameManager.Value;
        
        
        private void Awake()
        {
            GameStateManager.OnStateChanged += OnGameStateChanged;
            OnGameStateChanged(GameStateManager.GameState);
        }
        
        private void OnEnable()
        {
            _slingshotManager.OnReleased += HandleSlingshotReleased;
        }

        private void HandleSlingshotReleased()
        {
            _cameraController.ToFollowing();
            // _tiltController.StartListening();
        }

        private void OnDisable()
        {
            _slingshotManager.OnReleased -= HandleSlingshotReleased;
        }

        private void OnDestroy()
        {
            if (GameStateManager != null)
            {
                GameStateManager.OnStateChanged -= OnGameStateChanged;
            }
        }
        

        private void ToIdle()
        {
            _slingshotManager.BeginAiming();
        }

        
        private void ToNone()
        {
            _slingshotManager.StopAiming();
        }
        
        private void OnGameStateChanged(GameState newState)
        {
            if (newState == GameState.Playing)
            {
                ToIdle();
            }
            else
            {
                ToNone();
            }
        }

    }
}