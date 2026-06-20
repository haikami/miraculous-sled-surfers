using System;
using System.Collections.Generic;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Data.ScriptableObjects;
using SledSurfers.Scripts.Meta.Upgrades;

namespace SledSurfers.Scripts.Managers
{
    public class UpgradesManager
    {
        private readonly CurrencyManager _currencyManager;
        private readonly DataManager _dataManager;
        private readonly Dictionary<UpgradeType, UpgradeConfig> _upgrades = new();
        
        public UpgradesManager(UpgradeListConfig upgradeListConfig, CurrencyManager currencyManager, DataManager dataManager)
        {
            _currencyManager = currencyManager;
            _dataManager = dataManager;
            _upgrades = new();
            SetupUpgradesDictionary(upgradeListConfig);
        }

        private void SetupUpgradesDictionary(UpgradeListConfig upgradeListConfig)
        {
            var list = upgradeListConfig.UpgradesList;
            foreach (var upgradeConfig in list)
            {
                _upgrades[upgradeConfig.UpgradeType] = upgradeConfig;
            }
        }

        public string GetUpgradeDisplayValue(UpgradeType upgradeType)
        {
            var upgradeConfig = GetUpgradeConfig(upgradeType);
            return upgradeConfig.DisplayValue
                ? string.Format(upgradeConfig.DisplayValueFormat, GetUpgradeCurrentValue(upgradeConfig))
                : string.Empty;
        }
        
        
        public float GetUpgradeCurrentValueOrDefault(UpgradeType upgradeType, float defaultValue = 0f) => 
            _upgrades.TryGetValue(upgradeType, out var upgradeConfig) ? GetUpgradeCurrentValue(upgradeConfig) : defaultValue;

        public int GetUpgradeCurrentLevel(UpgradeType upgradeType)
            => _dataManager.GetUpgradeLevel(upgradeType);

        public float GetUpgradeCurrentValue(UpgradeConfig upgradeConfig)
        {
            var upgradeLevel = GetUpgradeCurrentLevel(upgradeConfig.UpgradeType);
            return upgradeConfig.GetUpgradeValue(upgradeLevel);
        }
        
        public UpgradeCostInfo GetUpgradeCostInfo(UpgradeType upgradeType)
        {
            if (!_upgrades.TryGetValue(upgradeType, out var upgradeConfig))
            {
                throw new InvalidOperationException($"[UpgradesManager] Upgrade type not available: {upgradeType}");
            }
            
            var currentLevel = GetUpgradeCurrentLevel(upgradeConfig.UpgradeType);
            var isMaxLevel = upgradeConfig.IsMaxLevel(currentLevel);
            var nextLevel = currentLevel + 1;
            
            if (isMaxLevel)
            {
                return new UpgradeCostInfo
                {
                    isMaxLevel = true,
                    currentLevel = currentLevel,
                };
            }

            var upgradeLevelCost = upgradeConfig.GetUpgradeCost(nextLevel);
            var canAffordCoins = _currencyManager.CanAfford(CurrencyType.Coins,upgradeLevelCost);
            if (canAffordCoins)
            {
                return new UpgradeCostInfo
                {
                    currencyType = CurrencyType.Coins,
                    upgradeCost = upgradeLevelCost,
                    canAfford = true,
                    isMaxLevel = false,
                    currentLevel = currentLevel,
                };
            }

            upgradeLevelCost = 1;
            var canAffordGems = _currencyManager.CanAfford(CurrencyType.Gems, upgradeLevelCost);

            return new UpgradeCostInfo
            {
                currencyType = CurrencyType.Gems,
                upgradeCost = 1,
                canAfford = canAffordGems,
                isMaxLevel = false,
                currentLevel = currentLevel,
            };
        }

        public UpgradeConfig GetUpgradeConfig(UpgradeType upgradeType)
        {
            if (!_upgrades.TryGetValue(upgradeType, out var upgradeConfig))
            {
                throw new InvalidOperationException($"[UpgradesManager] Upgrade type not available: {upgradeType}");
            }

            return upgradeConfig;
        }

        public bool TryUpgrade(UpgradeType upgradeType)
        {
            var upgradeCostInfo = GetUpgradeCostInfo(upgradeType);
            if (upgradeCostInfo.isMaxLevel || !upgradeCostInfo.canAfford || !_currencyManager.TrySpend(upgradeCostInfo.currencyType, upgradeCostInfo.upgradeCost))
            {
                return false;
            }
            _dataManager.IncreaseUpgradeLevel(upgradeType);
            _dataManager.SaveAsync();
            return true;
        }

        public IEnumerable<UpgradeType> GetUpgradeAvailableTypes() => _upgrades.Keys;
    }
}