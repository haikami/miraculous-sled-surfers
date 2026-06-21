using System.Threading.Tasks;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Data.Providers;
using SledSurfers.Scripts.Data.ScriptableObjects;
using SledSurfers.Scripts.Meta.Upgrades;

namespace SledSurfers.Scripts.Managers
{
    public class DataManager
    {
        private readonly IPlayerDataProvider _provider;
        private readonly LevelProgressionConfig _levelProgressionConfig;

        public PlayerData PlayerData { get; private set; }
        public int CurrentLevel => PlayerData.currentLevel;

        public DataManager(IPlayerDataProvider provider, LevelProgressionConfig levelProgressionConfig)
        {
            _provider = provider;
            _levelProgressionConfig = levelProgressionConfig;
        }

        public async Task LoadAsync()
        {
            PlayerData = await _provider.LoadAsync();
            //In case someone changed configs and a player already had a level that doesn't exist to avoid a soft lock
            if (!_levelProgressionConfig.IsValidLevel(PlayerData.currentLevel))
            {
                PlayerData.currentLevel = 0;
            }
        }

        public Task SaveAsync()
        {
            return _provider.SaveAsync(PlayerData);
        }
        
        public void UpdateMaxDistanceIfHigher(int distance)
        {
            if (distance <= PlayerData.maxDistanceReached) return;
            
            PlayerData.maxDistanceReached = distance;
        }

        public int GetUpgradeLevel(UpgradeType upgradeType) => PlayerData.upgrades.Find(x => x.upgradeType == upgradeType)?.level ?? 0;

        public void IncreaseUpgradeLevel(UpgradeType upgradeType)
        {
            var currentUpgradeData = PlayerData.upgrades.Find(x => x.upgradeType == upgradeType);
            if (currentUpgradeData == null)
            {
                PlayerData.upgrades.Add(new UpgradeSaveData() {  upgradeType = upgradeType, level = 1 });
            }
            else
            {
                currentUpgradeData.level++;
            }
        }

        public void ResetUpgrades() => PlayerData.upgrades.Clear();

        public void SetNextLevelData()
        {
            PlayerData.maxDistanceReached = 0;
            PlayerData.currentLevel = _levelProgressionConfig.GetNextLevelIndex(PlayerData.currentLevel);
        }
    }
}