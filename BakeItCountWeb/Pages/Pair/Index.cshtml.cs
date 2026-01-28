using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.Pair
{
    public class IndexModel : PageModel
    {
        public HttpClient _httpClient { get; set; }
        public List<PairDto> PairList { get; set; }

        public IndexModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("ApiClient");
        }
        public async Task OnGet()
        {
            PairList = await _httpClient.GetFromJsonAsync<List<PairDto>>("pair/GetAllPairsWithPurchase");
        }
    }
}
