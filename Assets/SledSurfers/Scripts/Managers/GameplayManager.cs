using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
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

        private GameplayStateManager _gameplayStateManager;

        private void Awake()
        {
            _gameplayStateManager = ServiceLocator.Get<GameplayStateManager>();
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
        
        private void OnRunEnded(RunResultData runResultData)
        {
            ServiceLocator.Get<RunResultManager>().SetLastRunResultData(runResultData);
            _gameplayStateManager.SwitchState(GameplayState.GameOver);
        }

        private void OnSlingshotReleased(Vector3 direction, float forcePercentage)
        {
            _cameraController.StartFollowing();
            _playerManager.Launch(direction, forcePercentage);
            _gameplayStateManager.SwitchState(GameplayState.Running);
        }
        
        public void StartGame()
        {
            _cameraController.ToIdleView();
            _slingshotManager.BeginAiming();
            _gameplayStateManager.SwitchState(GameplayState.Slingshot);
        }
    }
}