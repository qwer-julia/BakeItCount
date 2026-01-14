using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.Flavor
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("ApiClient");
        }
        public List<FlavorDto> Flavors { get; set; }

        public async Task OnGet()
        {
            Flavors = await _httpClient.GetFromJsonAsync<List<FlavorDto>>("flavor/allWithFlavors");

        }
    }
}
