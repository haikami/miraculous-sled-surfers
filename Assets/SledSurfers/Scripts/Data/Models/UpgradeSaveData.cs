using System;
using SledSurfers.Scripts.Meta.Upgrades;

namespace SledSurfers.Scripts.Data.Models
{
    [Serializable]
    public class UpgradeSaveData
    {
        public int level;
        public UpgradeType upgradeType;
    }
}