using ApiChooser.Interfaces;

namespace ApiChooser
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetAsync(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                throw new ApplicationException($"Error en la solicitud: {e.Message}", e);
            }
        }
    }
}
