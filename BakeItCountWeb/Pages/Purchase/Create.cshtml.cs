using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.Purchase
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public PurchaseCreateDto Purchase { get; set; }

        public ScheduleDto Schedule { get; set; }
        public List<FlavorDto> Flavors { get; set; }

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task OnGetAsync(int scheduleId)
        {
            Schedule = await _httpClient.GetFromJsonAsync<ScheduleDto>($"schedule/{scheduleId}");

            Purchase = new PurchaseCreateDto
            {
                ScheduleId = scheduleId,
                PurchaseDate = DateTime.Now
            };

            Flavors = await _httpClient.GetFromJsonAsync<List<FlavorDto>>("flavor/all");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Purchase.Flavor1Id == 0 || Purchase.Flavor2Id == 0)
            {
                ModelState.AddModelError(string.Empty, "Selecione ambos os sabores.");
            }

            if (!ModelState.IsValid)
            {
                Flavors = await _httpClient.GetFromJsonAsync<List<FlavorDto>>("flavor/all");
                return Page();
            }

            var response = await _httpClient.PostAsJsonAsync("purchase/create", Purchase);

            if (response.IsSuccessStatusCode)
            {
                var CurrentUser = await _httpClient.GetFromJsonAsync<UserDto>("auth/me");
                await _httpClient.GetFromJsonAsync<List<AchievementDto>>($"achievement/UpdatePurchaseAchievementsByUserId/{CurrentUser.UserId}");
                TempData["SuccessMessage"] = "Compra salva com sucesso!";
                return RedirectToPage("/Schedule/Details", new { id = Purchase.ScheduleId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao salvar a compra.");
                Flavors = await _httpClient.GetFromJsonAsync<List<FlavorDto>>("flavor/all");
                return Page();
            }
        }
    }
}
