using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Data.ScriptableObjects;
using SledSurfers.Scripts.Gameplay;
using SledSurfers.Scripts.Gameplay.Utils;
using SledSurfers.Scripts.Managers;
using SledSurfers.Scripts.Meta.Upgrades;
using UnityEngine;
using UnityEngine.Serialization;

namespace SledSurfers.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public event Action<RunResultData> OnRunEnded;

        [Header("Config")] 
        [SerializeField] private PlayerPhysicsConfig _config;
        
        [FormerlySerializedAs("_controller")]
        [Header("Subsystems")]
        [SerializeField] private PlayerLaunchController _launchController;
        [SerializeField] private PlayerMovementController _movement;
        [SerializeField] private PlayerTiltController _tilt;
        [SerializeField] private PlayerCollisionDetector _collision;
        
        [SerializeField] private PlayerMomentumTracker _momentumTracker;
        [SerializeField] private PlayerDistanceTracker _distanceTracker;
        
        private Vector3 _startPosition;

        private void OnEnable()
        {
            _momentumTracker.OnMomentumLost += HandleFinishLostMomentum;
            _collision.OnObstacleHit += HandleFinishObstacleHit;
        }

        private void OnDisable()
        {
            _momentumTracker.OnMomentumLost -= HandleFinishLostMomentum;
            _collision.OnObstacleHit -= HandleFinishObstacleHit;
        }
        
        public void Launch(Vector3 direction, float forcePercentage)
        {
            _launchController.Launch(direction, forcePercentage);
            _movement.StartListening();
            _tilt.StartRunning();
            _collision.StartListening();
            
            _momentumTracker.StartTracking();
            _distanceTracker.StartTracking();
        }

        public void SetPosition(Vector3 position) => transform.position = position;
        
        private void HandleFinishLostMomentum() => HandleLevelFailed(FinishReason.LostMomentum);
        private void HandleFinishObstacleHit() => HandleLevelFailed(FinishReason.Crashed);
        private void HandleLevelFailed(FinishReason reason)
        {
            _movement.StopListening();
            _tilt.StopRunning();
            _collision.StopListening();
            _launchController.StopRunning();
            
            _momentumTracker.StopTracking();
            _distanceTracker.StopTracking();

            var distanceTraveled = Mathf.RoundToInt(_distanceTracker.CurrentDistance);
            OnRunEnded?.Invoke(new RunResultData
            {
                reason = reason,
                distanceTraveled = distanceTraveled,
                currencies = PlayerLevelRewardsCalculator.GetLevelFailedRewards(
                    distanceTraveled,
                    ServiceLocator.Get<UpgradesManager>().GetUpgradeCurrentValueOrDefault(UpgradeType.CoinMultiplier, 1f)),
            });
        }

        public void SetupPlayer(Transform spawnPoint)
        {
            _distanceTracker.SetStartPoint(spawnPoint);
            _launchController.ResetPlayer(spawnPoint);
            ApplyConfig(_config);
        }

        private void ApplyConfig(PlayerPhysicsConfig config)
        {
            _tilt.SetConfig(config);
            _launchController.SetConfig(config);
            _momentumTracker.SetConfig(config);
        }

        public void ApplyUpgrades(float slingshotUpgradeValue, float sledUpgradeValue)
        {
            _launchController.SetExtraLaunchForceMultiplier(slingshotUpgradeValue);
            _tilt.SetLateralForceMultiplier(sledUpgradeValue);
        }
    }
}