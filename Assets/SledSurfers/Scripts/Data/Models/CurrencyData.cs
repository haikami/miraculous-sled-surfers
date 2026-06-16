using System;

namespace SledSurfers.Scripts.Data.Models
{
    public enum CurrencyType
    {
        Coins,
        Gems,
        Tickets
    }
    
    [Serializable]
    public struct CurrencyData
    {
        public CurrencyType currencyType;
        public int amount;
    }
}