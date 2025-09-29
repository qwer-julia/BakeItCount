using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.Purchase
{
    public class IndexModel : PageModel
    {
        public HttpClient _httpClient { get; set; }
        public List<PurchaseDto> PurchasesList { get; set; }

        public IndexModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public async Task OnGet()
        {
            PurchasesList = await _httpClient.GetFromJsonAsync<List<PurchaseDto>>("purchase/all");
        }
    }
}
