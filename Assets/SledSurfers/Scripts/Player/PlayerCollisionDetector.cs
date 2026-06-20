using System;
using SledSurfers.Scripts.Data.ScriptableObjects;
using SledSurfers.Scripts.Gameplay.Obstacles;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerCollisionDetector : MonoBehaviour, IPlayerConfigSetter
    {
        public event Action OnObstacleHit;          // crash — game over
        public event Action<float> OnMomentumLost;  // graze — pass new speed multiplier to apply

        [Header("References")]
        [SerializeField] private Rigidbody _rigidbody;
        
        private float _defaultMomentumPenalty;
        private float _maxGrazeAngle;
        private bool _hardObstaclesAlwaysCrash;

        private bool _isListening;

        public void StartListening() => _isListening = true;
        public void StopListening() => _isListening = false;

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isListening) return;

            var obstacle = collision.gameObject.GetComponent<ObstacleData>();
            if (obstacle == null) return; // not an obstacle, ignore

            if (IsCrash(collision, obstacle))
            {
                _isListening = false;
                OnObstacleHit?.Invoke();
            }
            else
            {
                var penalty = obstacle.MomentumPenaltyOverride >= 0f
                    ? obstacle.MomentumPenaltyOverride
                    : _defaultMomentumPenalty;

                ApplyMomentumPenalty(penalty);
            }
        }

        private bool IsCrash(Collision collision, ObstacleData obstacle)
        {
            if (obstacle.ObstacleType == ObstacleType.Hard && _hardObstaclesAlwaysCrash)
                return true;

            var contactNormal = collision.GetContact(0).normal;
            var angle = Vector3.Angle(-transform.forward, contactNormal);

            return angle <= _maxGrazeAngle;
        }

        private void ApplyMomentumPenalty(float penaltyFraction)
        {
            _rigidbody.velocity *= (1f - penaltyFraction);
            OnMomentumLost?.Invoke(1f - penaltyFraction);
        }

        public void SetConfig(PlayerPhysicsConfig config)
        {
            _hardObstaclesAlwaysCrash = config.HardObstaclesAlwaysCrash;
            _maxGrazeAngle = config.MaxGrazeAngle;
            _defaultMomentumPenalty = config.DefaultMomentumPenalty;
        }
    }
}