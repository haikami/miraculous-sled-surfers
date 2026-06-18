using System;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerCollisionDetector : MonoBehaviour
    {
        public event Action OnObstacleHit;

        public void StartListening()
        {
        }

        public void StopListening()
        {
        }
    }
}