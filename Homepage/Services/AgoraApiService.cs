using Homepage.Models;
using System.Net.Http.Json;

namespace Homepage.Services
{
    public class AgoraApiService : IDisposable
    {
        private readonly HttpClient _http;

        public AgoraApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _http = httpClient;

            //_http.BaseAddress ??= new Uri(configuration["API:BaseUri"]!);
        }

        public async Task<StatisticsDTO?> GetBotStatistics()
        {
            try
            {
                Console.WriteLine(_http.BaseAddress?.ToString());
                Console.WriteLine(_http.BaseAddress?.AbsoluteUri);

                return await _http.GetFromJsonAsync<StatisticsDTO>("api/stats");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Request Error - {e.GetType()}: {e.Message}");
                return null;
            }
        }

        public void Dispose()
        {
            _http?.Dispose();
        }
    }
}
