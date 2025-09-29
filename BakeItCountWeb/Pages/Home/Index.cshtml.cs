using BakeItCountWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace BakeItCountWeb.Pages.Home
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {

            bool isLogged = false;

            if (Request.Cookies.TryGetValue("AuthToken", out var token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(token);
                    isLogged = jwt.ValidTo > DateTime.Now;
                    if (isLogged)
                    {

                        return RedirectToPage("/Home/Authenticated");
                    }
                    else
                    {
                        return RedirectToPage("/Home/Login");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
            else {
                return RedirectToPage("/Home");
            }
        }
    }
}
