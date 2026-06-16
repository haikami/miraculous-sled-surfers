using System;
using UnityEngine;

namespace SledSurfers.Scripts.Data.Models
{
    [Serializable]
    public struct CollectableData
    {
        public CurrencyData currencyData;
        public Vector3 spawnPosition;
    }
}