using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.Purchase
{
    public class DetailsModel : PageModel
    {
        public HttpClient _httpClient { get; set; }
        public PurchaseDto Purchase { get; set; }

        public DetailsModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public async Task OnGet(int purchaseId)
        {
            Purchase = await _httpClient.GetFromJsonAsync<PurchaseDto>($"purchase/{purchaseId}");

        }
    }
}
