using System;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerMomentumTracker : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _minimumSpeedThreshold = 1.5f;
        
        public event Action OnMomentumLost;
        public float CurrentSpeed { get; private set; }

        
        private bool _isTracking;
        
        
        public void StartTracking()
        {
            _isTracking = true;
        }

        public void StopTracking()
        {
            _isTracking = false;
        }
        
        private void FixedUpdate()
        {
            if (!_isTracking) return;

            CurrentSpeed = _rigidbody.velocity.magnitude;

            if (CurrentSpeed < _minimumSpeedThreshold)
            {
                _isTracking = false;
                OnMomentumLost?.Invoke();
            }
        }    
    }
}