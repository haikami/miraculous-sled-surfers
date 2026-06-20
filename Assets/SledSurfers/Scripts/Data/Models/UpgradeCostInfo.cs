namespace SledSurfers.Scripts.Data.Models
{
    public struct UpgradeCostInfo
    {
        public int upgradeCost;
        public CurrencyType currencyType;
        public bool canAfford;
        public int currentLevel;
        public bool isMaxLevel;
    }
}