using SledSurfers.Scripts.Data.ScriptableObjects;
using UnityEngine;
using SledSurfers.Scripts.Gameplay.Input;

namespace SledSurfers.Scripts.Player
{
    public class PlayerTiltController : MonoBehaviour, IPlayerConfigSetter
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private DragInputDetector _input;

        
        private float _baseLateralForce = 5f;
        private float _maxLateralSpeed = 3f;
        private float _maxDragDistance = 200f;

        private float _lateralForceMultiplier = 1f;
        private float _normalizedX;
        private bool _hasPendingForce;

        public void SetLateralForceMultiplier(float multiplier)
        {
            _lateralForceMultiplier = multiplier;
        }

        public void StartRunning()
        {
            _normalizedX = 0f;
            _hasPendingForce = false;
            _input.OnDragged += HandleDragged;
            _input.OnDragReleased += HandleDragReleased;
            _input.Enable();
        }

        public void StopRunning()
        {
            _input.Disable();
            _input.OnDragged -= HandleDragged;
            _input.OnDragReleased -= HandleDragReleased;
            _normalizedX = 0f;
            _hasPendingForce = false;
        }

        private void HandleDragged(Vector2 offset)
        {
            var clampedX = Mathf.Clamp(offset.x, -_maxDragDistance, _maxDragDistance);
            _normalizedX = clampedX / _maxDragDistance;
            _hasPendingForce = true;
        }

        private void HandleDragReleased(Vector2 offset)
        {
            _normalizedX = 0f;
            _hasPendingForce = false;
        }

        private void FixedUpdate()
        {
            if (!_hasPendingForce) return;

            var currentLateralSpeed = Vector3.Dot(_rigidbody.velocity, transform.right);

            if (Mathf.Abs(currentLateralSpeed) < _maxLateralSpeed ||
                Mathf.Sign(currentLateralSpeed) != Mathf.Sign(_normalizedX))
            {
                var force = transform.right * (_normalizedX * _baseLateralForce * _lateralForceMultiplier);
                _rigidbody.AddForce(force, ForceMode.Force);
            }
        }

        public void SetConfig(PlayerPhysicsConfig config)
        {
            _baseLateralForce = config.BaseLateralForce;
            _maxLateralSpeed = config.MaxLateralSpeed;
            _maxDragDistance = config.MaxDragDistance;
        }
    }
}