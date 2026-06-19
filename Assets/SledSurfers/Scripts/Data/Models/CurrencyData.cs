using System;

namespace SledSurfers.Scripts.Data.Models
{
    public enum CurrencyType
    {
        Coins,
        Gems
    }
    
    [Serializable]
    public struct CurrencyData
    {
        public CurrencyType currencyType;
        public int amount;
    }
}