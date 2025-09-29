using BakeItCountApi.Cn.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BakeItCountApi.Cn.Login
{
    public class CnLogin
    {
        private readonly DaoUser _userDAO;
        private readonly IConfiguration _config;

        public CnLogin(DaoUser userDAO, IConfiguration config)
        {
            _userDAO = userDAO;
            _config = config;
        }

        public async Task<LoginResult> LoginAsync(LoginRequest login)
        {
            var user = await _userDAO.GetByEmailAsync(login.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.PasswordHash, user.PasswordHash))
                throw new UnauthorizedAccessException("Email ou senha inválidos.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:ExpiresInMinutes")),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return new LoginResult { Token = jwt, User = user };
        }
    }
}
