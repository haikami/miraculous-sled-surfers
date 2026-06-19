using System;
using SledSurfers.Scripts.Data.ScriptableObjects;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IPlayerConfigSetter
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rigidbody;
        
        public float CurrentSpeed => _rigidbody.velocity.z;

        private float _baseLaunchForceMultiplier;
        private float LaunchForceMultiplier => _baseLaunchForceMultiplier;

        public void StopRunning()
        {
            _rigidbody.isKinematic = true;
        }
        
        public void ResetPlayer(Transform spawnPoint)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.position = spawnPoint.position;
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }

        public void Launch(Vector3 direction, float forcePercentage)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(direction * (forcePercentage * LaunchForceMultiplier), ForceMode.Impulse);
        }

        public void SetConfig(PlayerPhysicsConfig config)
        {
            _rigidbody.mass = config.Mass;
            _rigidbody.drag = config.LinearDrag;
            _rigidbody.angularDrag = config.AngularDrag;
            _baseLaunchForceMultiplier = config.BaseLaunchForceMultiplier;
        }
    }
}