using System.Collections.Generic;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Meta.Upgrades;
using UnityEngine;

namespace SledSurfers.Scripts.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Config/Upgrades/Upgrade config", fileName = "UpgradesConfig")]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private UpgradeType _upgradeType;
        [SerializeField]private string _name;
        [SerializeField]private Sprite _icon;
        [SerializeField]private Color _color;
        
        [Header("Costs and values for each level")]
        [SerializeField] private UpgradeCostListConfig _costListConfig;
        [SerializeField] private UpgradeValuesListConfig _valuesListConfig;
        
        [Header("How this value would show inside an upgrade card. Leave blank to not display any value")]
        [SerializeField] private string _displayValueFormat;
        
        public UpgradeType UpgradeType => _upgradeType;
        public string Name => _name;
        public Sprite Icon => _icon;
        public Color Color => _color;
        public bool DisplayValue => !string.IsNullOrWhiteSpace(DisplayValueFormat);
        public string DisplayValueFormat =>  _displayValueFormat;
        
        private int NumLevels => _valuesListConfig.NumLevels;
        private int NumCosts => _costListConfig.NumCosts;
        
        
        public bool IsMaxLevel(int level) => level >= NumLevels;
        
        public float GetUpgradeValue(int level) => _valuesListConfig.GetValueAt(level);

        public int GetUpgradeCost(int level) => _costListConfig.GetValueAt(level);
    }
}