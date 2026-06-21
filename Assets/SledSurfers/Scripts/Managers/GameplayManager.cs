using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Gameplay;
using SledSurfers.Scripts.Gameplay.Level;
using SledSurfers.Scripts.Gameplay.Slingshot;
using SledSurfers.Scripts.Gameplay.Utils;
using SledSurfers.Scripts.Meta.Upgrades;
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

        private void OnRunSucceeded() => OnRunEnded(
            new RunResultData
            {
                reason = FinishReason.ReachedEnd,
                currencies = PlayerLevelRewardsCalculator.GetLevelCompletedRewards()
            });
        
        private void OnRunEnded(RunResultData runResultData)
        {
            ServiceLocator.Get<RunResultManager>().SetLastRunResultData(runResultData);
            StopListeningLevelEndTrigger();
            _cameraController.StopFollowing();
            _gameplayStateManager.SwitchState(GameplayState.GameOver);
        }

        private void OnSlingshotReleased(Vector3 direction, float forcePercentage)
        {
            _cameraController.StartFollowing();
            _playerManager.Launch(direction, forcePercentage);
            _gameplayStateManager.SwitchState(GameplayState.Running);
        }

        private void StartListeningLevelEndTrigger()
        {
            if (ServiceLocator.TryGet(out LevelDefinition levelDefinition))
            {
                levelDefinition.LevelEndTrigger.OnLevelEndReached += OnRunSucceeded;
            }
        }

        private void StopListeningLevelEndTrigger()
        {
            if (ServiceLocator.TryGet(out LevelDefinition levelDefinition))
            {
                levelDefinition.LevelEndTrigger.OnLevelEndReached -= OnRunSucceeded;
            }
        }
        
        public void StartGame()
        {
            _cameraController.ToIdleView();
            _slingshotManager.BeginAiming();
            _playerManager.SetPlayerPlayingState();
            ApplyUpgrades();
            StartListeningLevelEndTrigger();
            _gameplayStateManager.SwitchState(GameplayState.Slingshot);

        }

        private void ApplyUpgrades()
        {
            var upgradeManager = ServiceLocator.Get<UpgradesManager>();
            _playerManager.ApplyUpgrades(
                upgradeManager.GetUpgradeCurrentValueOrDefault(UpgradeType.Slingshot), 
                upgradeManager.GetUpgradeCurrentValueOrDefault(UpgradeType.Sled, 1f));
        }
    }
}