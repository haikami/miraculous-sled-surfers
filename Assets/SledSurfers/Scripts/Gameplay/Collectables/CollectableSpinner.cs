using System;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Collectables
{
    public class CollectableSpinner : MonoBehaviour
    {
        [SerializeField] private float _spinSpeed = 10f;
        
        private void Update()
        {
            transform.Rotate(0, _spinSpeed * Time.deltaTime, 0);
        }
    }
}