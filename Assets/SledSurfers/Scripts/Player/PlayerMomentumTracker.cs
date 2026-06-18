using System;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerMomentumTracker : MonoBehaviour
    {
        //Frames to allow physics apply force and update velocity before tracking it
        private const int PhysicsSettleFrames = 2;
        
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _minimumSpeedThreshold = 1.5f;
        
        public event Action OnMomentumLost;
        public float CurrentSpeed { get; private set; }
        
        private bool _isTracking;
        private int _framesSinceStart;

        public void StartTracking()
        {
            _isTracking = true;
            _framesSinceStart = 0;
        }
        
        public void StopTracking() => _isTracking = false;

        private void FixedUpdate()
        {
            if (!_isTracking) return;

            _framesSinceStart++;
            if (_framesSinceStart < PhysicsSettleFrames) return;

            CurrentSpeed = _rigidbody.velocity.magnitude;
            if (CurrentSpeed < _minimumSpeedThreshold)
            {
                _isTracking = false;
                OnMomentumLost?.Invoke();
            }
        }

    }
}