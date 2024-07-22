namespace Homepage.Services
{
    public class AgoraSecuredApiService(HttpClient httpClient) : IDisposable
    {
        private readonly HttpClient _http = httpClient;

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
