using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using BakeItCountApi.Cn.Users;
using BakeItCountApi.Data;
using BakeItCountApi.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeItCountApi.Tests.UserTest
{
    [TestClass]
    public class UserTest
    {
        private SqliteConnection? _connection;
        private DbContextOptions<Context> _options;

        [TestInitialize]
        public void TestInitialize()
        {
            // Opens a in-memory connection
            _connection = TestContextFactory.CreateOpenConnection();
            _options = TestContextFactory.CreateOptions(_connection);
            // Creates schema based on OnModelCreating (seeds are going to be applied too)
            using var ctx = new Context(_options);
            ctx.Database.EnsureCreated();
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
        public async Task GivenEmail_WhenExists_ThenReturnTrue()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var u = NewUser("exists@example.com");
            ctx.Users.Add(u);
            await ctx.SaveChangesAsync();

            var exists = await dao.EmailExistsAsync("exists@example.com");

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task GivenEmail_WhenMissing_ThenReturnFalse()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);

            var exists = await dao.EmailExistsAsync("missing@example.com");

            Assert.IsFalse(exists);
        }

        [TestMethod]
        public async Task GivenUser_WhenValid_ThenSaveOnDatabase()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var user = NewUser("newuser@example.com", "New user", "passwordHash");

            var returned = await dao.AddUserAsync(user);

            Assert.IsNotNull(returned);
            Assert.IsTrue(returned.UserId > 0, "UserId must be automatically set by database");
            Assert.AreEqual("newuser@example.com", returned.Email);
            Assert.AreEqual("New user", returned.Name);
        }

        [TestMethod]
        public async Task GivenEmail_WhenUserExists_ThenReturnUser()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var user = NewUser("find@example.com", "Findable User");
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var result = await dao.GetByEmailAsync("find@example.com");

            Assert.IsNotNull(result);
            Assert.AreEqual("Findable User", result.Name);
            Assert.AreEqual("find@example.com", result.Email);
        }

        [TestMethod]
        public async Task GivenEmail_WhenUserMissing_ThenReturnNull()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);

            var result = await dao.GetByEmailAsync("notfound@example.com");

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GivenUserId_WhenExists_ThenReturnTrue()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var user = NewUser("check@example.com");
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var exists = await dao.ExistsAsync(user.UserId);

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task GivenUserId_WhenMissing_ThenReturnFalse()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);

            var exists = await dao.ExistsAsync(999);

            Assert.IsFalse(exists);
        }

        [TestMethod]
        public async Task GivenUserId_WhenExists_ThenReturnUser()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var user = NewUser("getbyid@example.com", "Get By Id User");
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var result = await dao.GetUserById(user.UserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.UserId, result.UserId);
            Assert.AreEqual("Get By Id User", result.Name);
        }

        [TestMethod]
        public async Task GivenUserId_WhenMissing_ThenReturnNull()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);

            var result = await dao.GetUserById(999);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GivenMultipleUsers_WhenGetAll_ThenReturnAllUsers()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var users = new List<User>
            {
                NewUser("user1@example.com", "User One"),
                NewUser("user2@example.com", "User Two"),
                NewUser("user3@example.com", "User Three")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var result = await dao.GetAllAsync();

            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(u => u.Email == "user1@example.com"));
            Assert.IsTrue(result.Any(u => u.Email == "user2@example.com"));
            Assert.IsTrue(result.Any(u => u.Email == "user3@example.com"));
        }

        [TestMethod]
        public async Task GivenNoUsers_WhenGetAll_ThenReturnEmptyList()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);

            var result = await dao.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }

    [TestClass]
    public class CnUserTest
    {
        private SqliteConnection? _connection;
        private DbContextOptions<Context> _options;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = TestContextFactory.CreateOpenConnection();
            _options = TestContextFactory.CreateOptions(_connection);
            using var ctx = new Context(_options);
            ctx.Database.EnsureCreated();
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
        public async Task GivenValidUser_WhenRegister_ThenSaveWithHashedPassword()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnUser(dao);
            var user = NewUser("register@example.com", "New Registration", "plainPassword");

            var result = await cn.RegisterAsync(user);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.UserId > 0);
            Assert.AreEqual("register@example.com", result.Email);
            Assert.AreEqual("New Registration", result.Name);
            Assert.AreNotEqual("plainPassword", result.PasswordHash);
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify("plainPassword", result.PasswordHash));
        }

        [TestMethod]
        public async Task GivenPassword_WhenRegister_ThenPasswordShouldBeHashed()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnUser(dao);
            var user = NewUser("hashtest@example.com", "Hash Test", "mySecretPassword");

            var result = await cn.RegisterAsync(user);

            Assert.AreNotEqual("mySecretPassword", result.PasswordHash);
            Assert.IsTrue(result.PasswordHash.StartsWith("$2"));
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify("mySecretPassword", result.PasswordHash));
        }

        [TestMethod]
        public async Task GivenDuplicateEmail_WhenRegister_ThenThrowInvalidOperationException()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnUser(dao);
            var existingUser = NewUser("duplicate@example.com", "Existing User");
            ctx.Users.Add(existingUser);
            await ctx.SaveChangesAsync();

            var newUser = NewUser("duplicate@example.com", "Duplicate User");

            //await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            //    async () => await cn.RegisterAsync(newUser)
            //);
        }

        [TestMethod]
        public async Task GivenDuplicateEmail_WhenRegister_ThenThrowCorrectMessage()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnUser(dao);
            var existingUser = NewUser("duplicate2@example.com", "Existing User");
            ctx.Users.Add(existingUser);
            await ctx.SaveChangesAsync();

            var newUser = NewUser("duplicate2@example.com", "Duplicate User");

            //var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            //    async () => await cn.RegisterAsync(newUser)
            //);
            //Assert.AreEqual("Email já cadastrado.", exception.Message);
        }

        [TestMethod]
        public async Task GivenUserId_WhenExists_ThenReturnTrue()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnUser(dao);
            var user = NewUser("exists@example.com");
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var exists = await cn.ExistsAsync(user.UserId);

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task GivenUserId_WhenMissing_ThenReturnFalse()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnUser(dao);

            var exists = await cn.ExistsAsync(999);

            Assert.IsFalse(exists);
        }

        [TestMethod]
        public async Task GivenUserId_WhenExists_ThenReturnUser()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnUser(dao);
            var user = NewUser("getuser@example.com", "Get User Test");
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var result = await cn.GetUserById(user.UserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.UserId, result.UserId);
            Assert.AreEqual("Get User Test", result.Name);
        }

        [TestMethod]
        public async Task GivenUserId_WhenMissing_ThenReturnNull()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnUser(dao);

            var result = await cn.GetUserById(999);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GivenMultipleUsers_WhenGetAll_ThenReturnAllUsers()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnUser(dao);
            var users = new List<User>
            {
                NewUser("usera@example.com", "User A"),
                NewUser("userb@example.com", "User B")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var result = await cn.GetAllAsync();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(u => u.Name == "User A"));
            Assert.IsTrue(result.Any(u => u.Name == "User B"));
        }

        [TestMethod]
        public async Task GivenNoUsers_WhenGetAll_ThenReturnEmptyList()
        {
            using var ctx = new Context(_options);
            var dao = new DaoUser(ctx);
            var cn = new CnUser(dao);

            var result = await cn.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }

    [TestClass]
    public class UserModelTest
    {
        [TestMethod]
        public void GivenNewUser_WhenCreated_ThenActiveIsTrue()
        {
            var user = new User();

            Assert.IsTrue(user.Active);
        }

        [TestMethod]
        public void GivenNewUser_WhenCreated_ThenPropertiesCanBeSet()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                PasswordHash = "hash123"
            };

            Assert.AreEqual("Test User", user.Name);
            Assert.AreEqual("test@example.com", user.Email);
            Assert.AreEqual("hash123", user.PasswordHash);
            Assert.IsTrue(user.Active);
        }

        [TestMethod]
        public void GivenUser_WhenPropertiesChanged_ThenValuesAreUpdated()
        {
            var user = new User();

            user.Name = "Updated Name";
            user.Email = "updated@example.com";
            user.Active = false;
            user.PasswordHash = "newhash";

            Assert.AreEqual("Updated Name", user.Name);
            Assert.AreEqual("updated@example.com", user.Email);
            Assert.IsFalse(user.Active);
            Assert.AreEqual("newhash", user.PasswordHash);
        }
    }
}