using BakeItCountWeb.DTOs;

namespace BakeItCountWeb.Services
{
    public class CurrentUserService
    {
        private readonly HttpClient _httpClient;

        public CurrentUserService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            return await _httpClient.GetFromJsonAsync<UserDto>("auth/me");
        }
    }

}
