using System.Threading.Tasks;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Data.Providers;
using SledSurfers.Scripts.Meta.Upgrades;

namespace SledSurfers.Scripts.Managers
{
    public class DataManager
    {
        private readonly IPlayerDataProvider _provider;

        public PlayerData PlayerData { get; private set; }

        public DataManager(IPlayerDataProvider provider)
        {
            _provider = provider;
        }

        public async Task LoadAsync()
        {
            PlayerData = await _provider.LoadAsync();
        }

        public Task SaveAsync()
        {
            return _provider.SaveAsync(PlayerData);
        }
        
        public void UpdateMaxDistanceIfHigher(int distance)
        {
            if (distance <= PlayerData.maxDistanceReached) return;
            
            PlayerData.maxDistanceReached = distance;
            SaveAsync();
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
    }
}