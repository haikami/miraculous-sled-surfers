using System.Threading.Tasks;
using SledSurfers.Scripts.Data.Models;
using UnityEngine;

namespace SledSurfers.Scripts.Data.Providers
{
    public class LocalPlayerDataProvider : IPlayerDataProvider
    {
        private const string SaveKey = "player_data";

        public Task<PlayerData> LoadAsync()
        {
            var json = PlayerPrefs.GetString(SaveKey, null);

            var data = string.IsNullOrEmpty(json)
                ? new PlayerData()
                : JsonUtility.FromJson<PlayerData>(json);

            return Task.FromResult(data);
        }

        public Task SaveAsync(PlayerData data)
        {
            PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
            return Task.CompletedTask;
        }
    }
}