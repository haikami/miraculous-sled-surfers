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
        [SerializeField]private int _levelsPerMilestone;
        [SerializeField] private float _initialValue;
        [SerializeField]private List<UpgradeLevelData> _levels;
        [Header("How this value would show inside an upgrade card. Leave blank to not display any value")]
        [SerializeField] private string _displayValueFormat;
        
        public UpgradeType UpgradeType => _upgradeType;
        public string Name => _name;
        public Sprite Icon => _icon;
        public Color Color => _color;
        public int LevelsPerMilestone => _levelsPerMilestone;
        public bool DisplayValue => !string.IsNullOrWhiteSpace(DisplayValueFormat);
        public string DisplayValueFormat =>  _displayValueFormat;
        
        private int NumLevels => _levels.Count;
        
        
        public bool IsMaxLevel(int level) => level >= NumLevels - 1;
        
        public float GetUpgradeValue(int level)
        => level < 0 || NumLevels == 0 ? _initialValue : level < NumLevels ? _levels[level].value : _levels[^1].value;

        public int GetUpgradeCost(int level)
            => level > 0 && level < NumLevels ? _levels[level].cost : 0;
    }
}