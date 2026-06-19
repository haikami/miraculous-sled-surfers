using System;
using SledSurfers.Scripts.Data.ScriptableObjects;
using SledSurfers.Scripts.Gameplay;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public event Action<FinishReason> OnRunEnded;

        [Header("Config")] 
        [SerializeField] private PlayerPhysicsConfig _config;
        
        [Header("Subsystems")]
        [SerializeField] private PlayerController _controller;
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
            _controller.Launch(direction, forcePercentage);
            _movement.StartListening();
            _tilt.StartRunning();
            _collision.StartListening();
            
            _momentumTracker.StartTracking();
            _distanceTracker.StartTracking();
        }

        public void SetPosition(Vector3 position) => transform.position = position;
        
        private void HandleFinishLostMomentum() => HandleFinish(FinishReason.LostMomentum);
        private void HandleFinishObstacleHit() => HandleFinish(FinishReason.Crashed);
        private void HandleFinish(FinishReason reason)
        {
            _movement.StopListening();
            _tilt.StopRunning();
            _collision.StopListening();
            _controller.StopRunning();
            
            _momentumTracker.StopTracking();
            _distanceTracker.StopTracking();
            
            OnRunEnded?.Invoke(reason);
        }

        public void SetupPlayer(Transform spawnPoint)
        {
            _distanceTracker.SetStartPoint(spawnPoint);
            _controller.ResetPlayer(spawnPoint);
            ApplyConfig(_config);
        }

        private void ApplyConfig(PlayerPhysicsConfig config)
        {
            _tilt.SetConfig(config);
            _controller.SetConfig(config);
            _momentumTracker.SetConfig(config);
        }
    }
}