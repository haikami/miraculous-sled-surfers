using System;
using System.Collections.Generic;

namespace SledSurfers.Scripts.Data.Models
{
    [Serializable]
    public class PlayerData
    {
        public int currentLevel;
        public int maxDistanceReached;
        public Dictionary<CurrencyType, int> PersistentCurrencies = new();
        public Dictionary<CurrencyType, int> LevelCurrencies = new();
    }
}