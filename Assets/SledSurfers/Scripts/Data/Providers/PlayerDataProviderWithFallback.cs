using System;
using System.Threading.Tasks;
using SledSurfers.Scripts.Data.Models;
using UnityEngine;

namespace SledSurfers.Scripts.Data.Providers
{
    public class PlayerDataProviderWithFallback : IPlayerDataProvider
    {
        private readonly IPlayerDataProvider _primary;
        private readonly IPlayerDataProvider _fallback;

        public PlayerDataProviderWithFallback(
            IPlayerDataProvider primary,
            IPlayerDataProvider fallback)
        {
            _primary  = primary;
            _fallback = fallback;
        }

        public async Task<PlayerData> LoadAsync()
        {
            try
            {
                return await _primary.LoadAsync();
            }
            catch (Exception e) when (e is OperationCanceledException or TimeoutException)
            {
                Debug.LogWarning($"[PlayerData] Primary provider failed ({e.GetType().Name}), falling back to local.");
                return await _fallback.LoadAsync();
            }
        }

        public Task SaveAsync(PlayerData data)
        {
            // always save locally, fire-and-forget to server separately if needed
            return _fallback.SaveAsync(data);
        }
    }
}