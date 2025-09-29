using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BakeItCountWeb.Pages.Home
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        [BindProperty]
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loginRequest = new LoginRequest
            {
                Email = Email,
                PasswordHash = Password
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                    Response.Cookies.Append("AuthToken", result.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(1)
                    });
                    return RedirectToPage("/Home/Authenticated");

                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ErrorMessage = "Usuário ou senha inválidos.";
                    return Page();
                }
                else
                {
                    ErrorMessage = "Erro inesperado ao tentar logar.";
                    return Page();
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}

