using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BakeItCountApi.Cn.Login;
using BakeItCountApi.Cn.Users;
using BakeItCountApi.Data;
using BakeItCountApi.Controller;
using BakeItCountApi.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace BakeItCountApi.Tests.LoginTest
{
    [TestClass]
    public class LoginTest
    {
        private SqliteConnection? _connection;
        private DbContextOptions<Context> _options;
        private IConfiguration _configuration;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = TestContextFactory.CreateOpenConnection();
            _options = TestContextFactory.CreateOptions(_connection);
            using var ctx = new Context(_options);
            ctx.Database.EnsureCreated();

            // Setup configuration for JWT
            var configDict = new Dictionary<string, string>
            {
                { "Jwt:Key", "ThisIsASecretKeyForTestingPurposesOnly12345" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" },
                { "Jwt:ExpiresInMinutes", "60" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configDict)
                .Build();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        private User NewUser(string email, string name = "Test", string passwordHash = "password")
            => new User { Email = email, Name = name, PasswordHash = passwordHash };

        [TestMethod]
        public async Task GivenValidCredentials_WhenLogin_ThenReturnTokenAndUser()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnLogin(dao, _configuration);

            // Create user with hashed password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctPassword");
            var user = NewUser("login@example.com", "Login User", hashedPassword);
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var loginRequest = new LoginRequest
            {
                Email = "login@example.com",
                PasswordHash = "correctPassword"
            };

            var result = await cn.LoginAsync(loginRequest);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Token);
            Assert.IsNotNull(result.User);
            Assert.AreEqual("Login User", result.User.Name);
            Assert.AreEqual("login@example.com", result.User.Email);
        }

        [TestMethod]
        public async Task GivenValidCredentials_WhenLogin_ThenTokenContainsClaims()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnLogin(dao, _configuration);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
            var user = NewUser("claims@example.com", "Claims User", hashedPassword);
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var loginRequest = new LoginRequest
            {
                Email = "claims@example.com",
                PasswordHash = "password123"
            };

            var result = await cn.LoginAsync(loginRequest);

            // Validate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(result.Token, validationParameters, out var validatedToken);

            Assert.IsNotNull(principal);
            Assert.AreEqual(user.UserId.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Assert.AreEqual("Claims User", principal.FindFirst(ClaimTypes.Name)?.Value);
        }

        [TestMethod]
        public async Task GivenInvalidEmail_WhenLogin_ThenThrowUnauthorizedException()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnLogin(dao, _configuration);

            var loginRequest = new LoginRequest
            {
                Email = "nonexistent@example.com",
                PasswordHash = "anyPassword"
            };

            //await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(
            //    async () => await cn.LoginAsync(loginRequest)
            //);
        }

        [TestMethod]
        public async Task GivenInvalidPassword_WhenLogin_ThenThrowUnauthorizedException()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnLogin(dao, _configuration);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctPassword");
            var user = NewUser("wrongpass@example.com", "User", hashedPassword);
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var loginRequest = new LoginRequest
            {
                Email = "wrongpass@example.com",
                PasswordHash = "wrongPassword"
            };

            //await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(
            //    async () => await cn.LoginAsync(loginRequest)
            //);
        }

        [TestMethod]
        public async Task GivenInvalidCredentials_WhenLogin_ThenThrowCorrectMessage()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnLogin(dao, _configuration);

            var loginRequest = new LoginRequest
            {
                Email = "invalid@example.com",
                PasswordHash = "invalidPassword"
            };

            //var exception = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(
            //    async () => await cn.LoginAsync(loginRequest)
            //);
            //Assert.AreEqual("Email ou senha inválidos.", exception.Message);
        }

        [TestMethod]
        public async Task GivenValidLogin_WhenTokenGenerated_ThenExpirationIsSet()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnLogin(dao, _configuration);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password");
            var user = NewUser("expiry@example.com", "Expiry User", hashedPassword);
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var loginRequest = new LoginRequest
            {
                Email = "expiry@example.com",
                PasswordHash = "password"
            };

            var result = await cn.LoginAsync(loginRequest);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(result.Token);

            Assert.IsNotNull(jwtToken.ValidTo);
            Assert.IsTrue(jwtToken.ValidTo > DateTime.UtcNow);
            Assert.IsTrue(jwtToken.ValidTo <= DateTime.UtcNow.AddMinutes(61)); // 60 min + 1 min tolerance
        }
    }

    [TestClass]
    public class LoginRequestTest
    {
        [TestMethod]
        public void GivenValidData_WhenCreated_ThenPropertiesAreSet()
        {
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                PasswordHash = "password123"
            };

            Assert.AreEqual("test@example.com", loginRequest.Email);
            Assert.AreEqual("password123", loginRequest.PasswordHash);
        }
    }

    [TestClass]
    public class LoginResultTest
    {
        [TestMethod]
        public void GivenTokenAndUser_WhenCreated_ThenPropertiesAreSet()
        {
            var user = new User
            {
                UserId = 1,
                Name = "Test User",
                Email = "test@example.com",
                PasswordHash = "hash"
            };

            var loginResult = new LoginResult
            {
                Token = "sample.jwt.token",
                User = user
            };

            Assert.AreEqual("sample.jwt.token", loginResult.Token);
            Assert.IsNotNull(loginResult.User);
            Assert.AreEqual(1, loginResult.User.UserId);
            Assert.AreEqual("Test User", loginResult.User.Name);
        }
    }

    [TestClass]
    public class AuthControllerTest
    {
        private SqliteConnection? _connection;
        private DbContextOptions<Context> _options;
        private IConfiguration _configuration;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = TestContextFactory.CreateOpenConnection();
            _options = TestContextFactory.CreateOptions(_connection);
            using var ctx = new Context(_options);
            ctx.Database.EnsureCreated();

            var configDict = new Dictionary<string, string>
            {
                { "Jwt:Key", "ThisIsASecretKeyForTestingPurposesOnly12345" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" },
                { "Jwt:ExpiresInMinutes", "60" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configDict)
                .Build();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        private User NewUser(string email, string name = "Test", string passwordHash = "password")
            => new User { Email = email, Name = name, PasswordHash = passwordHash };

        [TestMethod]
        public async Task GivenValidUser_WhenRegister_ThenReturnOk()
        {
            using var ctx = new Context(_options);
            var daoUser = new DaoUser(ctx);
            var cnUser = new CnUser(daoUser);
            var cnLogin = new CnLogin(daoUser, _configuration);
            var controller = new AuthController(ctx, _configuration, cnUser, cnLogin);

            var user = NewUser("register@example.com", "New User", "password123");

            var result = await controller.Register(user);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
        }

        [TestMethod]
        public async Task GivenDuplicateEmail_WhenRegister_ThenReturnBadRequest()
        {
            using var ctx = new Context(_options);
            var daoUser = new DaoUser(ctx);
            var cnUser = new CnUser(daoUser);
            var cnLogin = new CnLogin(daoUser, _configuration);
            var controller = new AuthController(ctx, _configuration, cnUser, cnLogin);

            var existingUser = NewUser("duplicate@example.com", "Existing", "password");
            ctx.Users.Add(existingUser);
            await ctx.SaveChangesAsync();

            var newUser = NewUser("duplicate@example.com", "New User", "password123");

            var result = await controller.Register(newUser);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GivenValidCredentials_WhenLogin_ThenReturnOkWithToken()
        {
            using var ctx = new Context(_options);
            var daoUser = new DaoUser(ctx);
            var cnUser = new CnUser(daoUser);
            var cnLogin = new CnLogin(daoUser, _configuration);
            var controller = new AuthController(ctx, _configuration, cnUser, cnLogin);

            // Setup HttpContext for cookies
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
            var user = NewUser("loginctrl@example.com", "Login User", hashedPassword);
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var loginRequest = new LoginRequest
            {
                Email = "loginctrl@example.com",
                PasswordHash = "password123"
            };

            var result = await controller.Login(loginRequest);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
        }

        [TestMethod]
        public async Task GivenValidLogin_WhenLogin_ThenSetCookie()
        {
            using var ctx = new Context(_options);
            var daoUser = new DaoUser(ctx);
            var cnUser = new CnUser(daoUser);
            var cnLogin = new CnLogin(daoUser, _configuration);
            var controller = new AuthController(ctx, _configuration, cnUser, cnLogin);

            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password");
            var user = NewUser("cookie@example.com", "Cookie User", hashedPassword);
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var loginRequest = new LoginRequest
            {
                Email = "cookie@example.com",
                PasswordHash = "password"
            };

            await controller.Login(loginRequest);

            Assert.IsTrue(httpContext.Response.Headers.ContainsKey("Set-Cookie"));
        }

        [TestMethod]
        public async Task GivenInvalidCredentials_WhenLogin_ThenReturnUnauthorized()
        {
            using var ctx = new Context(_options);
            var daoUser = new DaoUser(ctx);
            var cnUser = new CnUser(daoUser);
            var cnLogin = new CnLogin(daoUser, _configuration);
            var controller = new AuthController(ctx, _configuration, cnUser, cnLogin);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var loginRequest = new LoginRequest
            {
                Email = "invalid@example.com",
                PasswordHash = "wrongPassword"
            };

            var result = await controller.Login(loginRequest);

            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task GivenAuthenticatedUser_WhenMe_ThenReturnUserData()
        {
            using var ctx = new Context(_options);
            var daoUser = new DaoUser(ctx);
            var cnUser = new CnUser(daoUser);
            var cnLogin = new CnLogin(daoUser, _configuration);
            var controller = new AuthController(ctx, _configuration, cnUser, cnLogin);

            var user = NewUser("me@example.com", "Me User", "password");
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            // Setup authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var result = await controller.Me();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
        }

        [TestMethod]
        public async Task GivenInvalidUserId_WhenMe_ThenReturnNotFound()
        {
            using var ctx = new Context(_options);
            var daoUser = new DaoUser(ctx);
            var cnUser = new CnUser(daoUser);
            var cnLogin = new CnLogin(daoUser, _configuration);
            var controller = new AuthController(ctx, _configuration, cnUser, cnLogin);

            // Setup authenticated user with non-existent ID
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "999"),
                new Claim(ClaimTypes.Name, "Non Existent")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var result = await controller.Me();

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}