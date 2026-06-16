using System.Threading.Tasks;
using SledSurfers.Scripts.Data.Models;

namespace SledSurfers.Scripts.Data.Providers
{
    public interface IPlayerDataProvider
    {
        Task<PlayerData> LoadAsync();
        Task SaveAsync(PlayerData data);
    }
}