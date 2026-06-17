using System;
using SledSurfers.Scripts.Gameplay;

namespace SledSurfers.Scripts.Data.Models
{
    [Serializable]
    public struct RunResultData
    {
        public FinishReason reason;
        public int distanceTraveled;
        public CurrencyData[] collectedCurrencies;
        public int score;
        public int distance;
    }
}