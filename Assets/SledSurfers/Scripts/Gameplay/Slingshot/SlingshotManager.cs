
using System;
using SledSurfers.Scripts.Core;
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
        private Vector2 _dragStartScreenPos;

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
            _dragStartScreenPos = screenPos;
        }

        private void HandleDragged(Vector2 screenPos)
        {
            var worldDelta = ScreenToWorldDelta(screenPos);
            var clamped = ClampToSlingshotBounds(worldDelta);
            _playerManager.SetPosition(_anchorPoint.position + clamped);
        }

        private void HandleDragReleased(Vector2 screenPos)
        {
            var worldDelta = ScreenToWorldDelta(screenPos);
            var clamped = ClampToSlingshotBounds(worldDelta);

            // pull-back direction is opposite to where you dragged, like a real slingshot
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

        private Vector3 ScreenToWorldDelta(Vector2 screenPos)
        {
            var startWorld = _camera.ScreenToWorldPoint(new Vector3(_dragStartScreenPos.x, _dragStartScreenPos.y, _camera.nearClipPlane + 10f));
            var currentWorld = _camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _camera.nearClipPlane + 10f));
            return currentWorld - startWorld;
        }

        private Vector3 ClampToSlingshotBounds(Vector3 delta)
        {
            var lateral = Mathf.Clamp(delta.x, -_maxLateralOffset, _maxLateralOffset);
            var pullBack = Mathf.Clamp(delta.z, -_maxReach, 0f); // assuming forward is +Z, only allow pulling back
            return new Vector3(lateral, 0f, pullBack);
        }
    }
}