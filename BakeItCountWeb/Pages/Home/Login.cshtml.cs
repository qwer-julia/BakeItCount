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
        [Required(ErrorMessage = "O e-mail � obrigat�rio")]
        [EmailAddress(ErrorMessage = "Informe um e-mail v�lido")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "A senha � obrigat�ria")]
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
                    var content = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(content))
                    {
                        ErrorMessage = "Resposta vazia da API";
                        return Page();
                    }

                    var result = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(content,
                        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result?.Token == null)
                    {
                        ErrorMessage = "Token não retornado pela API";
                        return Page();
                    }

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
                    ErrorMessage = "Usu�rio ou senha inv�lidos.";
                    return Page();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Erro: {response.StatusCode} - {errorContent}";
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

