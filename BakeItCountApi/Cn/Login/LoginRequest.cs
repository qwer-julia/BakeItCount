using System.ComponentModel.DataAnnotations;

namespace BakeItCountApi.Cn.Login
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // ou PasswordHash se você quiser
    }
}
