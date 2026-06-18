using System;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Rigidbody _rigidbody;

        [Header("Settings")]
        [SerializeField] private float _accelerationForce = 200f;
        [SerializeField] private float _rotationSpeed = 8f;
        [SerializeField] private float _groundCheckDistance = 5f;
        [SerializeField] private LayerMask _groundMask;

        private bool _isGrounded;
        private Vector3 _groundNormal = Vector3.up;
        
        private bool _isRunning;

        private void Awake()
        {
            // Let the Rigidbody handle translation only.
            _rigidbody.freezeRotation = true;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }


        public void StartListening()
        {
            _isRunning = true;
        }

        public void StopListening()
        {
            _isRunning = false;
        }
        
        private void FixedUpdate()
        {
            if (!_isRunning) return;
            
            UpdateGroundInfo();
            UpdateRotation();
        }

        private void UpdateGroundInfo()
        {
            RaycastHit hit;

            _isGrounded = Physics.Raycast(
                transform.position,
                Vector3.down,
                out hit,
                _groundCheckDistance,
                _groundMask);

            if (_isGrounded)
            {
                _groundNormal = hit.normal;
            }
        }

        private void UpdateRotation()
        {
            Vector3 velocity = _rigidbody.velocity;

            // Ignore tiny velocities
            if (velocity.sqrMagnitude < 0.1f)
                return;

            Vector3 forward = velocity.normalized;
            Vector3 up;

            if (_isGrounded)
            {
                // Follow the actual ground.
                up = _groundNormal;
            }
            else
            {
                // Predict the landing surface.
                RaycastHit hit;

                if (Physics.Raycast(
                        transform.position,
                        Vector3.down,
                        out hit,
                        50f,
                        _groundMask))
                {
                    up = hit.normal;
                }
                else
                {
                    up = Vector3.up;
                }
            }

            Quaternion targetRotation =
                Quaternion.LookRotation(forward, up);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.fixedDeltaTime);
        }
    }
}