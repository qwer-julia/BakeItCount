using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.Swap
{
    public class RespondModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public RespondModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public SwapRequest SwapRequest { get; set; }
        public ScheduleDto TargetSchedule { get; set; }
        public ScheduleDto SourceSchedule { get; set; }

        public UserDto CurrentUser { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SwapId { get; set; }

        [BindProperty]
        public int RespondingUserId { get; set; }

        [BindProperty]
        public bool Approve { get; set; }

        public async Task OnGet()
        {
            SwapRequest = await _httpClient.GetFromJsonAsync<SwapRequest>($"swap/{SwapId}");
            TargetSchedule = await _httpClient.GetFromJsonAsync<ScheduleDto>($"schedule/{SwapRequest.TargetScheduleId}");
            SourceSchedule = await _httpClient.GetFromJsonAsync<ScheduleDto>($"schedule/{SwapRequest.SourceScheduleId}");
            CurrentUser = await _httpClient.GetFromJsonAsync<UserDto>("auth/me");
        }

        public async Task<IActionResult> OnPost()
        {
            var request = new SwapResponseRequest
            {
                RespondingUserId = RespondingUserId,
                Approve = Approve
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"swap/{SwapId}/respond", request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Home/Authenticated");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return Page();
        }
    }

    public class SwapResponseRequest
    {
        public int RespondingUserId { get; set; }
        public bool Approve { get; set; }
    }
}
