using System.Collections.Generic;
using UnityEngine;

namespace SledSurfers.Scripts.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Config/Upgrades/Upgrade values", fileName = "UpgradeValuesConfig")]
    public class UpgradeValuesListConfig : ScriptableObject
    {
        //This would be used to display milestones
        [SerializeField]private int _levelsPerMilestone = 5;
        [SerializeField] private float _initialValue;
        [SerializeField] private List<float> _upgradeValues;
        public int NumLevels => _upgradeValues.Count;
        public int LevelsPerMilestone => _levelsPerMilestone;

        public float GetValueAt(int level) 
            => level < 0 || NumLevels == 0 ? _initialValue : level < NumLevels ? _upgradeValues[level] : _upgradeValues[^1];
    }
}