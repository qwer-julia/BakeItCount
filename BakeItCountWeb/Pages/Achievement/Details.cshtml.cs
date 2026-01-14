using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.Achievement
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public AchievementDto Achievement { get; set; }

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }
       
        public async Task OnGet(int achievementId)
        {
            Achievement = await _httpClient.GetFromJsonAsync<AchievementDto>($"achievement/GetAchievementById/{achievementId}");
        }
    }
}
