using System;
using System.Collections.Generic;

namespace SledSurfers.Scripts.Data.Models
{
    [Serializable]
    public class PlayerData
    {
        public int CurrentLevel;
        public int MaxDistanceReached;
        public List<CurrencyData> PersistentCurrencies = new();
        public List<CurrencyData> LevelCurrencies = new();
    }
}