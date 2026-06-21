using System;
using System.Threading;
using System.Threading.Tasks;
using SledSurfers.Scripts.Data.Models;
using UnityEngine;

namespace SledSurfers.Scripts.Data.Providers
{
    public class ServerPlayerDataProvider : IPlayerDataProvider
    {
        private readonly TimeSpan _timeout;

        public ServerPlayerDataProvider(TimeSpan timeout)
        {
            _timeout = timeout;
        }

        public async Task<PlayerData> LoadAsync()
        {
            // replace with your actual HTTP call
            using var cts = new CancellationTokenSource(_timeout);
            var response  = await FetchFromServerAsync(cts.Token);
            return response;
        }

        public async Task SaveAsync(PlayerData data)
        {
            // POST to server
            await Task.CompletedTask;
        }

        
        private async Task<PlayerData> FetchFromServerAsync(CancellationToken ct)
        {
            Debug.LogWarning("Server player data fetcher not implemented, throwing a timeout exception to emulate not reaching the servers");
            await Task.Delay(100, ct);
            throw new TimeoutException();
        }
    }
}