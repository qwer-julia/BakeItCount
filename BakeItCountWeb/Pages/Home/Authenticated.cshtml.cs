using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;

namespace BakeItCountWeb.Pages.Home
{
    public class AuthenticatedModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public AuthenticatedModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public UserDto CurrentUser { get; set; }
        public ScheduleDto NextSchedule { get; set; }
        public int PurchaseId { get; set; }
        public PairDto Pair { get; set; }
        public UserDto PairUser1 { get; set; }
        public UserDto PairUser2 { get; set; }
        public SwapRequest SwapRequest { get; set; }
        public List<FlavorDto> Flavors { get; set; }

        public async Task<IActionResult> OnGet()
        {
            CurrentUser = await _httpClient.GetFromJsonAsync<UserDto>("auth/me");
            NextSchedule = await _httpClient.GetFromJsonAsync<ScheduleDto>("schedule/next");
            PurchaseId = await _httpClient.GetFromJsonAsync<int>($"purchase/getPurchaseByScheduleId/{NextSchedule.ScheduleId}");
            Pair = await _httpClient.GetFromJsonAsync<PairDto>($"pair/{NextSchedule.PairId}");
            PairUser1 = await _httpClient.GetFromJsonAsync<UserDto>($"user/{Pair.UserId1}");
            PairUser2 = await _httpClient.GetFromJsonAsync<UserDto>($"user/{Pair.UserId2}");
            SwapRequest = await _httpClient.GetFromJsonAsync<SwapRequest>($"swap/getBySchedule/{NextSchedule.ScheduleId}") ?? null;
            Flavors = (await _httpClient.GetFromJsonAsync<List<FlavorDto>>("flavor/allWithFlavors"))
                .OrderByDescending(f => f.Votes) 
                .Take(4)                        
                .ToList();

            if (!Request.Cookies.TryGetValue("AuthToken", out var token))
            {
                return RedirectToPage("/Home/Login");
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                if (jwt.ValidTo < DateTime.UtcNow)
                {
                    return RedirectToPage("/Home/Login");
                }

            }
            catch
            {
                return RedirectToPage("/Home/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmScheduleAsync(int scheduleId)
        {
            var response = await _httpClient.PostAsync($"schedule/confirm/{scheduleId}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Confirmado com sucesso!";
                return RedirectToPage("/Home/Authenticated");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return Page();
        }
    }
}
