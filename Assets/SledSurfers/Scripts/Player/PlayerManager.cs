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
        [SerializeField] private PlayerMomentumTracker _momentum;
        [SerializeField] private PlayerCollisionDetector _collision;

        private void OnEnable()
        {
            _momentum.OnMomentumLost += HandleFinishLostMomentum;
            _collision.OnObstacleHit += HandleFinishObstacleHit;
        }

        private void OnDisable()
        {
            _momentum.OnMomentumLost -= HandleFinishLostMomentum;
            _collision.OnObstacleHit -= HandleFinishObstacleHit;
        }
        
        public void Launch(Vector3 force)
        {
            _controller.Launch(force);
            _movement.StartListening();
            _momentum.StartTracking();
            _tilt.StartListening();
            _collision.StartListening();
        }

        public void SetPosition(Vector3 position) => transform.position = position;
        
        private void HandleFinishLostMomentum() => HandleFinish(FinishReason.LostMomentum);
        private void HandleFinishObstacleHit() => HandleFinish(FinishReason.Crashed);
        private void HandleFinish(FinishReason reason)
        {
            _movement.StopListening();
            _momentum.StopTracking();
            _tilt.StopListening();
            _collision.StopListening();
            _controller.StopRunning();
            OnRunEnded?.Invoke(reason);
        }

        public void ResetPlayer(Transform spawnPoint) => _controller.ResetPlayer(spawnPoint);
    }
}