using Homepage.Models;

namespace Homepage.Shared.Interfaces
{
    public interface ISecuredApiService
    {
        Task<List<Guild>?> GetMutualGuildsAsync(string userId);
        Task<SettingsDTO> GetSettingsAsync(string userId);
    }
}