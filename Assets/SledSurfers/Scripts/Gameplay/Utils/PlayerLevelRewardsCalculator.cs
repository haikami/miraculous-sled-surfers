using System.Collections.Generic;
using SledSurfers.Scripts.Data.Models;
using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Utils
{
    public static class PlayerLevelRewardsCalculator
    {
        public static Dictionary<CurrencyType, int> GetLevelFailedRewards(int distanceTraveled, float coinsMultiplier)
        {
            var rewards = new Dictionary<CurrencyType, int>
            {
                [CurrencyType.Coins] = Mathf.RoundToInt(coinsMultiplier * distanceTraveled)
            };
            return rewards;
        }
        
        //TODO: this could have its own config based on the completed level
        public static Dictionary<CurrencyType, int> GetLevelCompletedRewards()
        {
            var rewards = new Dictionary<CurrencyType, int>
            {
                [CurrencyType.Gems] = 5
            };
            return rewards;
        }
    }
}