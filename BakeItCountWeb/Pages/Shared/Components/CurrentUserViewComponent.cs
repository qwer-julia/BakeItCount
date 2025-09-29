namespace BakeItCountWeb.Pages.Shared.CurrentUser
{
    using global::BakeItCountWeb.DTOs;
    using global::BakeItCountWeb.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Threading.Tasks;

    namespace BakeItCountWeb.Components
    {
        public class CurrentUserViewComponent : ViewComponent
        {
            private readonly CurrentUserService _currentUserService;
            private readonly IHttpClientFactory _httpClientFactory;

            public CurrentUserViewComponent(CurrentUserService currentUserService, IHttpClientFactory httpClient)
            {
                _currentUserService = currentUserService;
                _httpClientFactory = httpClient;
            }

            public async Task<IViewComponentResult> InvokeAsync()
            {
                var client = _httpClientFactory.CreateClient("ApiClient");

                UserDto? user = null;

                try
                {
                    user = await client.GetFromJsonAsync<UserDto>("auth/me");
                }
                catch
                {
                }

                return View(user);
            }
        }
    }

}
