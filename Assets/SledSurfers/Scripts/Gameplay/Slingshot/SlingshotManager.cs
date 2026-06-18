using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Gameplay.Input;
using SledSurfers.Scripts.Gameplay.Level;
using SledSurfers.Scripts.Player;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Slingshot
{
    public class SlingshotManager : MonoBehaviour
    {
        public event Action<Vector3> OnReleased;
        public event Action OnAimingCancelled;

        [Header("References")]
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private DragInputDetector _input;

        [Header("Settings")]
        [SerializeField] private float _maxReach = 2f;
        [SerializeField] private float _maxLateralOffset = 1f;
        [SerializeField] private float _launchForceMultiplier = 10f;

        private Transform _anchorPoint;
        private Camera _camera;

        public void BeginAiming()
        {
            _anchorPoint = ServiceLocator.Get<LevelDefinition>()?.PlayerSpawnPoint;
            _camera = Camera.main;
            _input.OnDragStarted += HandleDragStarted;
            _input.OnDragged += HandleDragged;
            _input.OnDragReleased += HandleDragReleased;
            _input.Enable();
        }

        public void StopAiming()
        {
            _input.Disable();
            _input.OnDragStarted -= HandleDragStarted;
            _input.OnDragged -= HandleDragged;
            _input.OnDragReleased -= HandleDragReleased;
        }

        private void HandleDragStarted(Vector2 screenPos)
        {
            // anchor point in screen space for reference if needed, but
            // offset computation is now owned by DragInputDetector
        }

        private void HandleDragged(Vector2 screenOffset)
        {
            var worldDelta = ScreenOffsetToWorldDelta(screenOffset);
            var clamped = ClampToSlingshotBounds(worldDelta);
            _playerManager.SetPosition(_anchorPoint.position + clamped);
        }

        private void HandleDragReleased(Vector2 screenOffset)
        {
            var worldDelta = ScreenOffsetToWorldDelta(screenOffset);
            var clamped = ClampToSlingshotBounds(worldDelta);

            var launchDirection = -clamped.normalized;
            var launchForce = clamped.magnitude * _launchForceMultiplier;

            var minimumForceReached = true;

            if (minimumForceReached)
            {
                StopAiming();
                OnReleased?.Invoke(launchDirection * launchForce);
            }
            else
            {
                OnAimingCancelled?.Invoke();
            }
        }

        private Vector3 ScreenOffsetToWorldDelta(Vector2 screenOffset)
        {
            // convert a screen-space pixel offset to a world-space delta
            // by comparing world position of screen center vs screen center + offset
            var screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, _camera.nearClipPlane + 10f);
            var screenCenterWorld = _camera.ScreenToWorldPoint(screenCenter);
            var offsetWorld = _camera.ScreenToWorldPoint(screenCenter + new Vector3(screenOffset.x, screenOffset.y, 0f));
            return offsetWorld - screenCenterWorld;
        }

        private Vector3 ClampToSlingshotBounds(Vector3 delta)
        {
            var lateral = Mathf.Clamp(delta.x, -_maxLateralOffset, _maxLateralOffset);
            var pullBack = Mathf.Clamp(delta.z, -_maxReach, 0f);
            return new Vector3(lateral, 0f, pullBack);
        }
    }
}