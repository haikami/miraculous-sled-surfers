using System.Threading.Tasks;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Data.Providers;

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
    }
}