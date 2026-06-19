using System;
using System.Collections.Generic;
using SledSurfers.Scripts.Gameplay;

namespace SledSurfers.Scripts.Data.Models
{
    [Serializable]
    public struct RunResultData
    {
        public FinishReason reason;
        public int distanceTraveled;
        public Dictionary<CurrencyType, int> currencies;
    }
}