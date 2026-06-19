using System;
using System.Collections.Generic;

namespace SledSurfers.Scripts.Data.Models
{
    [Serializable]
    public class PlayerData
    {
        public int currentLevel;
        public int maxDistanceReached;
        public List<CurrencyData> persistentCurrencies = new();
        public List<CurrencyData> levelCurrencies = new();
    }
}