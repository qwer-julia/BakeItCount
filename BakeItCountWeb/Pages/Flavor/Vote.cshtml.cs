using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

public class VoteModel : PageModel
{
    private readonly HttpClient _httpClient;

    public VoteModel(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient("ApiClient");
    }

    [BindProperty] public string UserId { get; set; }
    [BindProperty] public List<int> SelectedFlavors { get; set; }
    public List<FlavorDto> Flavors { get; set; }

    public async Task OnGetAsync()
    {
        var currentUser = await _httpClient.GetFromJsonAsync<UserDto>("auth/me");

        UserId = currentUser.UserId.ToString();
        Flavors = await _httpClient.GetFromJsonAsync<List<FlavorDto>>("flavor/all");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (SelectedFlavors == null || !SelectedFlavors.Any())
        {
            ModelState.AddModelError("", "Selecione pelo menos 1 sabor.");
        }

        if (SelectedFlavors?.Count > 2)
        {
            ModelState.AddModelError("", "Você pode escolher no máximo 2 sabores.");
        }

        if (!ModelState.IsValid)
        {
            Flavors = await _httpClient.GetFromJsonAsync<List<FlavorDto>>("flavor/all");
            return Page();
        }

        var payload = new { userId = UserId, flavors = SelectedFlavors };
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("flavor/vote", content);

        if (response.IsSuccessStatusCode)
        {
            TempData["Message"] = "Voto registrado com sucesso!";
            return RedirectToPage("Index");
        }

        ModelState.AddModelError("", "Erro ao registrar voto.");
        Flavors = await _httpClient.GetFromJsonAsync<List<FlavorDto>>("flavor/all");
        return Page();
    }
}
