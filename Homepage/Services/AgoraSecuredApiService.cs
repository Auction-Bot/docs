using Homepage.Models;
using Homepage.Shared.Interfaces;
using System.Net.Http.Json;

namespace Homepage.Services
{
    public class AgoraSecuredApiService : IDisposable, ISecuredApiService
    {
        private readonly HttpClient _http;

        private const string UserGuildsUri = "/api/discord/guilds/{0}";

        public AgoraSecuredApiService(HttpClient httpClient) => _http = httpClient;

        public async Task<List<Guild>?> GetMutualGuildsAsync(string userId)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<Guild>>(string.Format(UserGuildsUri, userId));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Request Error - {e.GetType()}: {e.Message}");
                return null;
            }
        }

        public Task<SettingsDTO> GetSettingsAsync(string userId)
        {
            throw new NotImplementedException();
        }
        public void Dispose() => _http?.Dispose();
    }
}
