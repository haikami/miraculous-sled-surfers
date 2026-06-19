using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.ScriptableObjects;
using SledSurfers.Scripts.Gameplay.Input;
using SledSurfers.Scripts.Gameplay.Level;
using SledSurfers.Scripts.Player;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Slingshot
{
    public class SlingshotManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private DragInputDetector _input;
        [SerializeField] private SlingshotConfig _config;
     
        public event Action<Vector3, float> OnReleased;
        public event Action OnAimingCancelled;
        public event Action<float> OnPullPercentageChanged;

        private Transform _anchorPoint;
        private Camera _camera;

        public void BeginAiming()
        {
            _anchorPoint = ServiceLocator.Get<LevelDefinition>()?.PlayerSpawnPoint;
            _camera = Camera.main;
            _input.OnDragged += HandleDragged;
            _input.OnDragReleased += HandleDragReleased;
            _input.Enable();
        }

        public void StopAiming()
        {
            _input.Disable();
            _input.OnDragged -= HandleDragged;
            _input.OnDragReleased -= HandleDragReleased;
        }

        private void HandleDragged(Vector2 screenOffset)
        {
            var worldDelta = ScreenOffsetToWorldDelta(screenOffset);
            var clamped = ClampToUShape(worldDelta);

            _playerManager.SetPosition(_anchorPoint.position + clamped);

            var percentage = CalculatePullPercentage(clamped);
            Debug.Log("Pull: "+Mathf.RoundToInt(percentage*100f)+"%");
            OnPullPercentageChanged?.Invoke(percentage);
        }

        private void HandleDragReleased(Vector2 screenOffset)
        {
            var worldDelta = ScreenOffsetToWorldDelta(screenOffset);
            var clamped = ClampToUShape(worldDelta);

            var percentage = CalculatePullPercentage(clamped);
            var launchDirection = clamped.sqrMagnitude > 0.0001f ? -clamped.normalized : Vector3.back;

            if (percentage >= _config.MinimumLaunchPercentage)
            {
                StopAiming();
                OnReleased?.Invoke(launchDirection, percentage);
            }
            else
            {
                OnAimingCancelled?.Invoke();
            }
        }

        private Vector3 ScreenOffsetToWorldDelta(Vector2 screenOffset)
        {
            var screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, _camera.nearClipPlane + 10f);
            var screenCenterWorld = _camera.ScreenToWorldPoint(screenCenter);
            var offsetWorld = _camera.ScreenToWorldPoint(screenCenter + new Vector3(screenOffset.x, screenOffset.y, 0f));
            return offsetWorld - screenCenterWorld;
        }

        /// Clamps a raw pull-back delta to the U-shaped boundary:
        /// two straight vertical sides down to _straightDepth, then a
        /// semicircle of radius _maxLateralOffset closing the bottom.
        private Vector3 ClampToUShape(Vector3 delta)
        {
            var x = delta.x;
            var z = Mathf.Min(delta.z, 0f); // only allow pulling back

            var straightDepth = _config.StraightDepth;
            var radius = _config.MaxLateralOffset;

            // above the curve's start depth — straight side region
            if (z >= -straightDepth)
            {
                var clampedX = Mathf.Clamp(x, -radius, radius);
                return new Vector3(clampedX, 0f, z);
            }

            // below straight depth — semicircle region, centered at (0, -straightDepth)
            var curveCenter = new Vector2(0f, -straightDepth);
            var offsetFromCenter = new Vector2(x, z) - curveCenter;

            if (offsetFromCenter.magnitude > radius)
                offsetFromCenter = offsetFromCenter.normalized * radius;

            var clampedPoint = curveCenter + offsetFromCenter;
            return new Vector3(clampedPoint.x, 0f, clampedPoint.y);
        }

        /// Maps a clamped U-shape position to a tension percentage, based on
        /// straight-line distance from the top-center anchor (0,0). Max distance
        /// (100%) is straight down to the bottom of the semicircle.
        private float CalculatePullPercentage(Vector3 clamped)
        {
            var maxDistance = _config.StraightDepth + _config.MaxLateralOffset;
            if (maxDistance <= 0f) return 0f;

            var distanceFromAnchor = new Vector2(clamped.x, clamped.z).magnitude;
            return Mathf.Clamp01(distanceFromAnchor / maxDistance) * _config.MaxPercentage;
        }
    }
}