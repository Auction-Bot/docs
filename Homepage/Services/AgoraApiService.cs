using Homepage.Models;
using Homepage.Shared.Interfaces;
using System.Net.Http.Json;

namespace Homepage.Services
{
    public class AgoraApiService : IDisposable, IApiService
    {
        private readonly HttpClient _http;

        private const string StatisticsUri = "api/stats";

        public AgoraApiService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<StatisticsDTO?> GetBotStatistics()
        {
            try
            {
                return await _http.GetFromJsonAsync<StatisticsDTO>(StatisticsUri);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Request Error - {e.GetType()}: {e.Message}");
                return null;
            }
        }

        public void Dispose() => _http?.Dispose();
    }
}
