using System;
using SledSurfers.Scripts.Gameplay;
using SledSurfers.Scripts.Gameplay.Slingshot;
using SledSurfers.Scripts.Gameplay.UI;
using SledSurfers.Scripts.Player;
using UnityEngine;

namespace SledSurfers.Scripts.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        public event Action<FinishReason> OnGameFinished;
        
        [Header("References")] 
        [SerializeField] private SlingshotManager _slingshotManager;
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private CameraController _cameraController;
        
        [Header("UI")] 
        [SerializeField] private ResultScreen _resultScreen;
        
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
            _resultScreen.Show(()=> OnGameFinished?.Invoke(finishReason));
        }

        private void OnSlingshotReleased(Vector3 direction, float forcePercentage)
        {
            _cameraController.StartFollowing();
            _playerManager.Launch(direction, forcePercentage);
            // _tiltController.StartListening();
        }
        
        
        public void StartGame()
        {
            _cameraController.ToIdleView();
            _slingshotManager.BeginAiming();
        }
    }
}