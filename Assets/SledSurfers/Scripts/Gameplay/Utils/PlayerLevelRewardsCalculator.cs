using System.Collections.Generic;
using SledSurfers.Scripts.Data.Models;

namespace SledSurfers.Scripts.Gameplay.Utils
{
    public static class PlayerLevelRewardsCalculator
    {
        public static Dictionary<CurrencyType, int> GetLevelFailedRewards(int distanceTraveled)
        {
            var rewards = new Dictionary<CurrencyType, int>();
            rewards[CurrencyType.Coins] = distanceTraveled;
            return rewards;
        }
        
        public static Dictionary<CurrencyType, int> GetLevelCompletedRewards(int level)
        {
            var rewards = new Dictionary<CurrencyType, int>();
            rewards[CurrencyType.Gems] = 500;
            return rewards;
        }
    }
}