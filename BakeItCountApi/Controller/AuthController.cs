using BakeItCountApi.Cn.Login;
using BakeItCountApi.Cn.Users;
using BakeItCountApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BakeItCountApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Context _context;
        private readonly IConfiguration _config;
        private readonly CnUser _userCN;
        private readonly CnLogin _loginCn;

        public AuthController(Context context, IConfiguration config, CnUser userCN, CnLogin loginCn)
        {
            _context = context;
            _config = config;
            _userCN = userCN;
            _loginCn = loginCn;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                var createdUser = await _userCN.RegisterAsync(user);
                return Ok(new { createdUser.UserId, createdUser.Name, createdUser.Email });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            try
            {
                var result = await _loginCn.LoginAsync(login);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                };

                Response.Cookies.Append("AuthToken", result.Token, cookieOptions);

                return Ok(new { token = result.Token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();
            return Ok(new { user.UserId, user.Name, user.Email });
        }
    }
}
