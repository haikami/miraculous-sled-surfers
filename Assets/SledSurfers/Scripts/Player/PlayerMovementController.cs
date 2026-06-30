using System;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        public event Action<bool> OnGroundStateChanged;

        [Header("References")]
        [SerializeField] private Rigidbody _rigidbody;

        [Header("Settings")]
        [SerializeField] private float _rotationSpeed = 8f;
        [SerializeField] private float _groundCheckDistance = 5f;
        [SerializeField] private float _landingPredictionDistance = 50f;
        [SerializeField] private LayerMask _groundMask;

        private bool _isRunning;
        private bool _isGrounded;
        private Vector3 _groundNormal = Vector3.up;

        private void Awake()
        {
            _rigidbody.freezeRotation = true;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        public void StartListening()
        {
            _isRunning = true;
            ResetGroundState();
        }

        public void StopListening()
        {
            _isRunning = false;
            ResetGroundState();
        }

        private void ResetGroundState()
        {
            var wasGrounded = _isGrounded;
            _isGrounded = true;
            _groundNormal = Vector3.up;

            _rigidbody.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);

            if (!wasGrounded)
                OnGroundStateChanged?.Invoke(true);
        }

        private void FixedUpdate()
        {
            if (!_isRunning) return;

            UpdateGroundInfo();
            UpdateRotation();
        }

        private void UpdateGroundInfo()
        {
            var wasGrounded = _isGrounded;
            _isGrounded = Physics.Raycast(
                transform.position,
                Vector3.down,
                out var hit,
                _groundCheckDistance,
                _groundMask);

            _groundNormal = _isGrounded ? hit.normal : Vector3.up;

            if (wasGrounded != _isGrounded)
                OnGroundStateChanged?.Invoke(_isGrounded);
        }

        private void UpdateRotation()
        {
            var velocity = _rigidbody.velocity;
            if (velocity.sqrMagnitude < 0.1f) return;

            var forward = velocity.normalized;
            var up = _isGrounded ? _groundNormal : PredictLandingNormal();

            var targetRotation = Quaternion.LookRotation(forward, up);
            var newRotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);

            _rigidbody.MoveRotation(newRotation);
        }

        private Vector3 PredictLandingNormal()
        {
            return Physics.Raycast(transform.position, Vector3.down, out var hit, _landingPredictionDistance, _groundMask)
                ? hit.normal
                : Vector3.up;
        }
    }
}