using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace BakeItCountWeb.Pages.Home
{
    public class AuthenticatedModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthenticatedModel> _logger;

        public AuthenticatedModel(
            IHttpClientFactory httpClientFactory,
            ILogger<AuthenticatedModel> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _logger = logger;
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
            try
            {
                _logger.LogInformation("=== Iniciando carregamento da página Authenticated ===");

                if (!Request.Cookies.TryGetValue("AuthToken", out var token))
                {
                    _logger.LogWarning("Token não encontrado nos cookies");
                    return RedirectToPage("/Home/Login");
                }

                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(token);

                    if (jwt.ValidTo < DateTime.UtcNow)
                    {
                        _logger.LogWarning("Token expirado");
                        return RedirectToPage("/Home/Login");
                    }

                    _logger.LogInformation("Token válido");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao validar token");
                    return RedirectToPage("/Home/Login");
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                _logger.LogInformation("Buscando dados do usuário...");

                CurrentUser = await _httpClient.GetFromJsonAsync<UserDto>("auth/me");
                _logger.LogInformation($"Usuário carregado: {CurrentUser?.Name}");

                _logger.LogInformation("Buscando próximo schedule...");
                NextSchedule = await _httpClient.GetFromJsonAsync<ScheduleDto>("schedule/next");

                if (NextSchedule == null)
                {
                    _logger.LogWarning("Nenhum schedule encontrado");
                    TempData["InfoMessage"] = "Nenhum agendamento encontrado.";
                    return Page();
                }

                _logger.LogInformation($"Schedule carregado: {NextSchedule.ScheduleId}");

                _logger.LogInformation("Buscando purchase...");
                PurchaseId = await _httpClient.GetFromJsonAsync<int>($"purchase/getPurchaseByScheduleId/{NextSchedule.ScheduleId}");
                _logger.LogInformation($"Purchase ID: {PurchaseId}");

                _logger.LogInformation("Buscando pair...");
                Pair = await _httpClient.GetFromJsonAsync<PairDto>($"pair/{NextSchedule.PairId}");
                _logger.LogInformation($"Pair carregado: {Pair?.PairId}");

                _logger.LogInformation("Buscando usuários do pair...");
                PairUser1 = await _httpClient.GetFromJsonAsync<UserDto>($"user/{Pair.UserId1}");
                PairUser2 = await _httpClient.GetFromJsonAsync<UserDto>($"user/{Pair.UserId2}");
                _logger.LogInformation($"Usuários carregados: {PairUser1?.Name}, {PairUser2?.Name}");

                _logger.LogInformation("Buscando swap request...");
                try
                {
                    SwapRequest = await _httpClient.GetFromJsonAsync<SwapRequest>($"swap/getBySchedule/{NextSchedule.ScheduleId}");
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("Nenhum swap request encontrado (404)");
                    SwapRequest = null;
                }

                _logger.LogInformation("Buscando sabores...");
                Flavors = (await _httpClient.GetFromJsonAsync<List<FlavorDto>>("flavor/allWithFlavors"))
                    ?.OrderByDescending(f => f.Votes)
                    .Take(4)
                    .ToList() ?? new List<FlavorDto>();

                _logger.LogInformation($"Total de sabores carregados: {Flavors?.Count ?? 0}");

                _logger.LogInformation("=== Página Authenticated carregada com sucesso ===");
                return Page();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Erro HTTP ao carregar dados. Status: {ex.StatusCode}");
                TempData["ErrorMessage"] = $"Erro ao carregar dados: {ex.Message}";

                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToPage("/Home/Login");
                }

                return RedirectToPage("/Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao carregar página Authenticated");
                TempData["ErrorMessage"] = "Erro ao carregar a página. Tente novamente.";
                return RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostConfirmScheduleAsync(int scheduleId)
        {
            try
            {
                _logger.LogInformation($"Confirmando schedule: {scheduleId}");

                if (Request.Cookies.TryGetValue("AuthToken", out var token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.PostAsync($"schedule/confirm/{scheduleId}", null);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Schedule {scheduleId} confirmado com sucesso");
                    TempData["SuccessMessage"] = "Confirmado com sucesso!";
                    return RedirectToPage("/Home/Authenticated");
                }

                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro ao confirmar schedule: {error}");
                ModelState.AddModelError(string.Empty, error);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao confirmar schedule");
                ModelState.AddModelError(string.Empty, "Erro ao confirmar. Tente novamente.");
                return Page();
            }
        }
    }
}