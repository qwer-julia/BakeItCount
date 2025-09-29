using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeItCountWeb.Pages.Schedule
{
    public class IndexModel : PageModel
    {
        public HttpClient _httpClient { get; set; }
        public List<ScheduleDto> SchedulesList { get; set; } 

        public IndexModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public async Task OnGet()
        {
            SchedulesList = await _httpClient.GetFromJsonAsync<List<ScheduleDto>>("schedules/GetAllSchedules");

        }
    }
}
