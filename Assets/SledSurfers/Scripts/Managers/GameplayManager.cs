using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
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
        [SerializeField] private ResultView _resultView;
        
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
        
        private void OnRunEnded(RunResultData runResultData)
        {
            _resultView.Show(runResultData, ()=>
            {
                ServiceLocator.Get<CurrencyManager>().Add(runResultData.currencies);
                OnGameFinished?.Invoke(runResultData.reason);
            });
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