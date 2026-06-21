using System;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Level
{
    public class LevelEndTrigger : MonoBehaviour
    {
        public event Action OnLevelEndReached;

        private bool _hasTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasTriggered || !other.CompareTag("Player")) return;

            _hasTriggered = true;
            OnLevelEndReached?.Invoke();
        }

        public void Reset()
        {
            _hasTriggered = false;
        }
    }
}