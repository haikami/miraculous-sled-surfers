using System.Collections.Generic;
using UnityEngine;

namespace SledSurfers.Scripts.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Config/Upgrades/Upgrades list", fileName = "UpgradesList")]
    public class UpgradeListConfig : ScriptableObject
    {
        [SerializeField] private List<UpgradeConfig> _upgradesList;
        
        public List<UpgradeConfig> UpgradesList => _upgradesList;
    }
}