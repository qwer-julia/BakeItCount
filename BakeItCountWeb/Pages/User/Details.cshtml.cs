using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.User
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public UserDto User { get; set; }
        public PairDto Pair { get; set; }
        public List<AchievementDto> UserAchievements { get; set; }
        public FlavorDto FavoriteFlavor { get; set; }

        public List<PurchaseDto> PurchaseList { get; set; }

        public async Task OnGetAsync(int userId)
        {
            User = await _httpClient.GetFromJsonAsync<UserDto>($"user/{userId}");
            UserAchievements = await _httpClient.GetFromJsonAsync<List<AchievementDto>>($"achievement/GetAchievementsByUserId/{userId}");
            Pair = await _httpClient.GetFromJsonAsync<PairDto>($"pair/getPairByUserId/{userId}");
            UserAchievements = await _httpClient.GetFromJsonAsync<List<AchievementDto>>($"achievement/GetAchievementsByUserId/{userId}");
            PurchaseList = await _httpClient.GetFromJsonAsync<List<PurchaseDto>>($"purchase/getPurchaseByUserId/{userId}");
            FavoriteFlavor = await _httpClient.GetFromJsonAsync<FlavorDto>($"flavor/favoriteFlavor/{userId}");
        }
    }
}
