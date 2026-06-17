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
    }
}