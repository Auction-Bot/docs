using System.Net.Http.Json;

namespace Homepage.Services
{
    public class AgoraSecuredApiService(HttpClient httpClient) : IDisposable
    {
        private readonly HttpClient _http = httpClient;

        private const string PAT_Uri = "api/PersonalAccessToken";

        public async Task<string> GetPersonalAccessToken() => await _http.GetFromJsonAsync<string>(PAT_Uri) ?? string.Empty;

        public void Dispose() => _http?.Dispose();
    }
}
