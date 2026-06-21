using System;
using SledSurfers.Scripts.Player;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Cameras
{
    public enum CameraMode
    {
        MainMenu,
        Idle,
        Following,
        Frozen
    }

    [System.Serializable]
    public struct OrbitSettings
    {
        public float angle;
        public float distance;
        public float height;
    }

    public class CameraController : MonoBehaviour
    {
        public event Action OnTransitionCompleted;
        private const float PositionSettleThreshold = 0.05f;
        private const float AngleSettleThreshold = 0.5f;

        [Header("References")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private PlayerMomentumTracker _playerMomentumTracker;

        [Header("Orbit Views")]
        [SerializeField] private OrbitSettings _mainMenuOrbit = new OrbitSettings { angle = 180f, distance = 4f, height = 3f };
        [SerializeField] private OrbitSettings _idleOrbit = new OrbitSettings { angle = 0f, distance = 8f, height = 4f };

        [Header("Following")]
        [SerializeField] private Vector3 _followOffset = new Vector3(0, 4, -8);

        [Header("Blend")]
        [SerializeField] private float _orbitBlendSpeed = 2f;
        [SerializeField] private float _positionSmoothSpeed = 5f;
        [SerializeField] private float _rotationSmoothSpeed = 5f;

        [Header("Speed FX")]
        [SerializeField] private float _baseFov = 60f;
        [SerializeField] private float _maxFov = 75f;
        [SerializeField] private float _speedForMaxFov = 20f;
        [SerializeField] private float _fovSmoothSpeed = 4f;

        private Transform Target => _playerTransform;

        private CameraMode _mode = CameraMode.Idle;
        private Transform _orbitPivot;

        private OrbitSettings _currentOrbit;
        private OrbitSettings _targetOrbit;

        private bool _isOrbitSettled;

        public void ToIdleView()
        {
            _mode = CameraMode.Idle;
            _orbitPivot = Target;
            _targetOrbit = _idleOrbit;
            _isOrbitSettled = false;
        }

        public void ToMainMenuView(Transform playerSpawnPoint)
        {
            _mode = CameraMode.MainMenu;
            _orbitPivot = playerSpawnPoint;
            _targetOrbit = _mainMenuOrbit;
            _isOrbitSettled = false;
        }

        public void SnapToMainMenuView(Transform playerSpawnPoint)
        {
            ToMainMenuView(playerSpawnPoint);
            _currentOrbit = _targetOrbit;

            var rotation = Quaternion.Euler(0f, _currentOrbit.angle, 0f);
            var offset = rotation * new Vector3(0f, _currentOrbit.height, -_currentOrbit.distance);
            transform.position = playerSpawnPoint.position + offset;
            transform.rotation = Quaternion.LookRotation(playerSpawnPoint.position - transform.position, Vector3.up);

            _isOrbitSettled = true; // already there, no transition needed
        }

        public void StartFollowing()
        {
            _mode = CameraMode.Following;
        }

        public void StopFollowing()
        {
            _mode = CameraMode.Frozen;
        }

        private void LateUpdate()
        {
            switch (_mode)
            {
                case CameraMode.MainMenu:
                case CameraMode.Idle:
                    UpdateOrbit();
                    break;
                case CameraMode.Following:
                    UpdateFollow();
                    break;
                case CameraMode.Frozen:
                    break; // hold position, no recompute at all
            }

            UpdateSpeedFx();
        }

        private void UpdateOrbit()
        {
            if (_isOrbitSettled)
                return; // already locked in place — don't recompute from a moving pivot

            _currentOrbit.angle = Mathf.LerpAngle(_currentOrbit.angle, _targetOrbit.angle, _orbitBlendSpeed * Time.deltaTime);
            _currentOrbit.distance = Mathf.Lerp(_currentOrbit.distance, _targetOrbit.distance, _orbitBlendSpeed * Time.deltaTime);
            _currentOrbit.height = Mathf.Lerp(_currentOrbit.height, _targetOrbit.height, _orbitBlendSpeed * Time.deltaTime);

            var rotation = Quaternion.Euler(0f, _currentOrbit.angle, 0f);
            var offset = rotation * new Vector3(0f, _currentOrbit.height, -_currentOrbit.distance);

            var pivotPos = _orbitPivot != null ? _orbitPivot.position : Target.position;
            var desiredPosition = pivotPos + offset;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, _positionSmoothSpeed * Time.deltaTime);

            var lookRotation = Quaternion.LookRotation(pivotPos - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotationSmoothSpeed * Time.deltaTime);

            if (IsOrbitConverged())
            {
                _isOrbitSettled = true;
                OnTransitionCompleted?.Invoke();
            }
        }

        private bool IsOrbitConverged()
        {
            var angleDiff = Mathf.Abs(Mathf.DeltaAngle(_currentOrbit.angle, _targetOrbit.angle));
            var distanceDiff = Mathf.Abs(_currentOrbit.distance - _targetOrbit.distance);
            return angleDiff < AngleSettleThreshold && distanceDiff < PositionSettleThreshold;
        }

        private void UpdateFollow()
        {
            var desiredPosition = Target.position + Target.TransformDirection(_followOffset);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _positionSmoothSpeed * Time.deltaTime);

            var lookRotation = Quaternion.LookRotation(Target.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotationSmoothSpeed * Time.deltaTime);
        }

        private void UpdateSpeedFx()
        {
            if (_mode != CameraMode.Following || _camera == null) return;

            var speed = _playerMomentumTracker != null ? _playerMomentumTracker.CurrentSpeed : 0f;
            var speedRatio = Mathf.Clamp01(speed / _speedForMaxFov);
            var targetFov = Mathf.Lerp(_baseFov, _maxFov, speedRatio);

            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFov, _fovSmoothSpeed * Time.deltaTime);
        }
    }
}