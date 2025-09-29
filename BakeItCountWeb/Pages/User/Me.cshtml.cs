using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.User
{
    public class MeModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public MeModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public UserDto CurrentUser { get; set; }
        public PairDto Pair { get; set; }
        public List<AchievementDto> UserAchievements { get; set; }
        public FlavorDto FavoriteFlavor { get; set; }

        public List<PurchaseDto> PurchaseList { get; set; }

        public async Task OnGetAsync()
        {
            CurrentUser = await _httpClient.GetFromJsonAsync<UserDto>("auth/me");
            Pair = await _httpClient.GetFromJsonAsync<PairDto>($"pair/getPairByUserId/{CurrentUser.UserId}");
            UserAchievements = await _httpClient.GetFromJsonAsync<List<AchievementDto>>($"achievement/GetAchievementsByUserId/{CurrentUser.UserId}");
            PurchaseList = await _httpClient.GetFromJsonAsync<List<PurchaseDto>>($"purchase/getPurchaseByUserId/{CurrentUser.UserId}");
            FavoriteFlavor = await _httpClient.GetFromJsonAsync<FlavorDto>($"flavor/favoriteFlavor/{CurrentUser.UserId}");
        }
    }
}
