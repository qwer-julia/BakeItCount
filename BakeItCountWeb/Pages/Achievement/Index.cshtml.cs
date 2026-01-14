using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.Achievement
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<AchievementDto> AchievementList { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task OnGet()
        {
            AchievementList = await _httpClient.GetFromJsonAsync<List<AchievementDto>>("achievement/all");
        }
    }
}
