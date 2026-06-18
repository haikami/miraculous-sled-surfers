using System;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rigidbody;
        
        public void ResetPlayer(Transform spawnPoint)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.position = spawnPoint.position;
        }

        public void Launch(Vector3 direction, float force)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}