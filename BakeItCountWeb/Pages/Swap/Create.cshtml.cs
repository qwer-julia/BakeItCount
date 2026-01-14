using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.Swap
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public PairDto Pair { get; set; }
        public UserDto CurrentUser { get; set; }
        public List<PairDto> PairsList { get; set; }
        public ScheduleDto Schedule { get; set; }

        [BindProperty]
        public SwapRequest SwapRequest { get; set; }

        public async Task OnGet(int scheduleId)
        {
            Schedule = await _httpClient.GetFromJsonAsync<ScheduleDto>($"schedule/{scheduleId}");
            CurrentUser = await _httpClient.GetFromJsonAsync<UserDto>("auth/me");
            Pair = await _httpClient.GetFromJsonAsync<PairDto>($"pair/getPairByUserId/{CurrentUser.UserId}");
            PairsList = await _httpClient.GetFromJsonAsync<List<PairDto>>("pair/all");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("swap/request", SwapRequest);

            if (response.IsSuccessStatusCode)
            {
                var CurrentUser = await _httpClient.GetFromJsonAsync<UserDto>("auth/me");
                await _httpClient.GetFromJsonAsync<List<AchievementDto>>($"achievement/UpdatePurchaseAchievementsByUserId/{CurrentUser.UserId}");

                var createdSwap = await response.Content.ReadFromJsonAsync<SwapRequest>();

                if (createdSwap != null)
                {
                    SwapRequest.SwapId = createdSwap.SwapId;

                    return RedirectToPage("/Swap/Respond", new { swapId = SwapRequest.SwapId });
                }
            }

            ModelState.AddModelError(string.Empty, "Erro ao enviar solicitação de troca.");
            return Page();
        }
    }
}
