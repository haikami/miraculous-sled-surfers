using System;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rigidbody;

        [Header("Settings")] [SerializeField] private float _minAllowedVelocity; 
        
        public float CurrentSpeed => _rigidbody.velocity.z;

        private bool _isRunning;
        
        public void ResetPlayer(Transform spawnPoint)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.position = spawnPoint.position;
            _isRunning = false;
        }

        public void Launch(Vector3 force)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(force, ForceMode.Impulse);
            _isRunning = true;
        }
    }
}