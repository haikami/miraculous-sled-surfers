using System;
using SledSurfers.Scripts.Gameplay;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public event Action<FinishReason> OnRunEnded;

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
        
        public void Launch(Vector3 force)
        {
            _controller.Launch(force);
            _movement.StartListening();
            _tilt.StartListening();
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
            _tilt.StopListening();
            _collision.StopListening();
            _controller.StopRunning();
            
            _momentumTracker.StopTracking();
            _distanceTracker.StopTracking();
            
            OnRunEnded?.Invoke(reason);
        }

        public void ResetPlayer(Transform spawnPoint)
        {
            _distanceTracker.SetStartPoint(spawnPoint);
            _controller.ResetPlayer(spawnPoint);
        }
    }
}