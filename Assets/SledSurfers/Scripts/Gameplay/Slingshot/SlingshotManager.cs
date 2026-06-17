
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Slingshot
{
    public class SlingshotManager : MonoBehaviour
    {
        [SerializeField] private DragInputDetector _input;
        [SerializeField] private Transform _anchorPoint;
        [SerializeField] private Transform _playerVisual;
        [SerializeField] private float _maxReach = 2f;
        [SerializeField] private float _maxLateralOffset = 1f;
        [SerializeField] private float _launchForceMultiplier = 10f;

        private Camera _camera;
        private Vector2 _dragStartScreenPos;

        public void BeginAiming()
        {
            _camera = Camera.main;
            _input.OnDragStarted += HandleDragStarted;
            _input.OnDragged += HandleDragged;
            _input.OnDragReleased += HandleDragReleased;
            _input.Enable();
        }

        private void HandleDragStarted(Vector2 screenPos)
        {
            _dragStartScreenPos = screenPos;
        }

        private void HandleDragged(Vector2 screenPos)
        {
            var worldDelta = ScreenToWorldDelta(screenPos);
            var clamped = ClampToSlingshotBounds(worldDelta);
            _playerVisual.position = _anchorPoint.position + clamped;
        }

        private void HandleDragReleased(Vector2 screenPos)
        {
            var worldDelta = ScreenToWorldDelta(screenPos);
            var clamped = ClampToSlingshotBounds(worldDelta);

            // pull-back direction is opposite to where you dragged, like a real slingshot
            var launchDirection = -clamped.normalized;
            var launchForce = clamped.magnitude * _launchForceMultiplier;

            StopAiming();
            ApplyLaunch(launchDirection, launchForce);
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

        private void StopAiming()
        {
            _input.Disable();
            _input.OnDragStarted -= HandleDragStarted;
            _input.OnDragged -= HandleDragged;
            _input.OnDragReleased -= HandleDragReleased;
        }

        private void ApplyLaunch(Vector3 direction, float force)
        {
            //ServiceLocator.Get<PlayerController>().Launch(direction, force);
        }
    }
}