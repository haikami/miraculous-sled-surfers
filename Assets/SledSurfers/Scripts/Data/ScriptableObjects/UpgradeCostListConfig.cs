using System.Collections.Generic;
using UnityEngine;

namespace SledSurfers.Scripts.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Config/Upgrades/Upgrade costs", fileName = "UpgradeCosts")]
    public class UpgradeCostListConfig : ScriptableObject
    {
        [SerializeField] private List<int> _upgradeCosts;
        public int NumCosts => _upgradeCosts.Count;

        //If there are more upgrades than costs then the last upgrades will always cost the last element of this list
        public int GetValueAt(int level) 
            => level <= 0 ? 0 : level < NumCosts ? _upgradeCosts[level] : _upgradeCosts[^1];
    }
}