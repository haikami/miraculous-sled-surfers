using System;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerDistanceTracker : MonoBehaviour
    {
        public float CurrentDistance { get; private set; }
        private bool _isTracking;
        private Vector3 _startPosition;
        private Vector3 _startForward;

        public void StartTracking()
        {
            _isTracking = true;
        }

        public void StopTracking()
        {
            _isTracking = false;
        }

        public void SetStartPoint(Transform spawnPoint)
        {
            _startPosition = spawnPoint.position;
            _startForward = spawnPoint.forward;
            CurrentDistance = 0f;
        }

        private void Update()
        {
            if (!_isTracking) return;
            
            var flatDelta = Vector3.ProjectOnPlane(transform.position - _startPosition, Vector3.up);
            var flatForward = Vector3.ProjectOnPlane(_startForward, Vector3.up);

            CurrentDistance =
                Vector3.Dot(flatDelta, flatForward) > 0f
                    ? flatDelta.magnitude
                    : 0f;
                
            Debug.Log("Distance: "+CurrentDistance);
        }
    }
}