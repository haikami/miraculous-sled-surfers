using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Gameplay;
using SledSurfers.Scripts.Gameplay.Slingshot;
using SledSurfers.Scripts.Player;
using UnityEngine;

namespace SledSurfers.Scripts.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private SlingshotManager _slingshotManager;
        [SerializeField] private PlayerManager _playerManager;
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
            _slingshotManager.OnReleased += OnSlingshotReleased;
            _playerManager.OnRunEnded += OnRunEnded;
        }

        
        private void OnDisable()
        {
            _slingshotManager.OnReleased -= OnSlingshotReleased;
            _playerManager.OnRunEnded -= OnRunEnded;
        }
        
        private void OnRunEnded(FinishReason finishReason)
        {
            
        }


        private void OnSlingshotReleased(Vector3 force)
        {
            _cameraController.StartFollowing();
            _playerManager.Launch(force);
            // _tiltController.StartListening();
        }



        private void OnDestroy()
        {
            if (GameStateManager != null)
            {
                GameStateManager.OnStateChanged -= OnGameStateChanged;
            }
        }
        

        private void StartGame()
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
                StartGame();
            }
            else
            {
                ToNone();
            }
        }

    }
}