using System;
using System.Collections.Generic;
using SledSurfers.Scripts.Data.Models;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Collectables
{
    public class CollectableMarker : MonoBehaviour
    {
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private int _currencyAmount = 1;

        private void Awake()
        {
            //Avoid showing marker if it has any visuals
            gameObject.SetActive(false);
        }

        public CollectableData GetCollectableData()
            => new CollectableData
            {
                currencyData = new CurrencyData
                {
                    currencyType = _currencyType,
                    amount = _currencyAmount
                },
                spawnPosition = transform.position
            };
        
#if UNITY_EDITOR
        private static readonly Dictionary<CurrencyType, Color> _gizmoColors = new()
        {
            { CurrencyType.Coins, Color.yellow },
            { CurrencyType.Gems,  Color.cyan   },
            { CurrencyType.Tickets, Color.green },
        };
        
        private void OnDrawGizmos()
        {
            var color = _gizmoColors.TryGetValue(_currencyType, out var c) ? c : Color.white;

            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, 0.5f);
            //TODO: replace spheres by icons
            // Gizmos.DrawIcon(transform.position, "coin_icon.png", allowScaling: true);


            // draw amount label above it
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.Label(
                transform.position + Vector3.up * 0.5f,
                $"{_currencyType} x{_currencyAmount}"
            );
        }
#endif
    }
}