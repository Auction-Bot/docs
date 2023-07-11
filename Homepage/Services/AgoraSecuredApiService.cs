using Homepage.Models;
using System.Net.Http.Json;

namespace Homepage.Services
{
    public class AgoraSecuredApiService : IDisposable
    {
        private readonly HttpClient _http;

        public AgoraSecuredApiService(HttpClient httpClient) => _http = httpClient;

        public void Dispose()
        {
            _http?.Dispose();
        }
    }
}
