using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using BakeItCountApi.Cn.Pairs;
using BakeItCountApi.Cn.Users;
using BakeItCountApi.Cn.Schedules;
using BakeItCountApi.Data;
using BakeItCountApi.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeItCountApi.Tests.PairTest
{
    [TestClass]
    public class DaoPairTest
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

        private User NewUser(string email, string name = "Test")
            => new User { Email = email, Name = name, PasswordHash = "hash" };

        private Pair NewPair(int userId1, int userId2, int order = 1)
            => new Pair { UserId1 = userId1, UserId2 = userId2, Order = order };

        [TestMethod]
        public async Task GivenPair_WhenAdd_ThenSaveToDatabase()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var user1 = NewUser("user1@example.com", "User One");
            var user2 = NewUser("user2@example.com", "User Two");
            ctx.Users.AddRange(user1, user2);
            await ctx.SaveChangesAsync();

            var pair = NewPair(user1.UserId, user2.UserId);

            var result = await dao.AddAsync(pair);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.PairId > 0);
            Assert.AreEqual(user1.UserId, result.UserId1);
            Assert.AreEqual(user2.UserId, result.UserId2);
        }

        [TestMethod]
        public async Task GivenUserId_WhenUserInPair_ThenReturnPair()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var user1 = NewUser("existing1@example.com", "Existing One");
            var user2 = NewUser("existing2@example.com", "Existing Two");
            ctx.Users.AddRange(user1, user2);
            await ctx.SaveChangesAsync();

            var pair = NewPair(user1.UserId, user2.UserId);
            ctx.Pairs.Add(pair);
            await ctx.SaveChangesAsync();

            var result = await dao.GetExistingPairForUsers(user1.UserId, 0);

            Assert.IsNotNull(result);
            Assert.AreEqual(pair.PairId, result.PairId);
            Assert.IsNotNull(result.User1);
            Assert.IsNotNull(result.User2);
        }

        [TestMethod]
        public async Task GivenUserId_WhenUserNotInPair_ThenReturnNull()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);

            var result = await dao.GetExistingPairForUsers(999, 0);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GivenTwoUserIds_WhenEitherInPair_ThenReturnPair()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var user1 = NewUser("check1@example.com", "Check One");
            var user2 = NewUser("check2@example.com", "Check Two");
            ctx.Users.AddRange(user1, user2);
            await ctx.SaveChangesAsync();

            var pair = NewPair(user1.UserId, user2.UserId);
            ctx.Pairs.Add(pair);
            await ctx.SaveChangesAsync();

            var result = await dao.GetExistingPairForUsers(user1.UserId, user2.UserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(pair.PairId, result.PairId);
        }

        [TestMethod]
        public async Task GivenPairId_WhenExists_ThenReturnPair()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var user1 = NewUser("getbyid1@example.com", "Get One");
            var user2 = NewUser("getbyid2@example.com", "Get Two");
            ctx.Users.AddRange(user1, user2);
            await ctx.SaveChangesAsync();

            var pair = NewPair(user1.UserId, user2.UserId);
            ctx.Pairs.Add(pair);
            await ctx.SaveChangesAsync();

            var result = await dao.GetByIdAsync(pair.PairId);

            Assert.IsNotNull(result);
            Assert.AreEqual(pair.PairId, result.PairId);
            Assert.IsNotNull(result.User1);
            Assert.IsNotNull(result.User2);
            Assert.AreEqual("Get One", result.User1.Name);
            Assert.AreEqual("Get Two", result.User2.Name);
        }

        [TestMethod]
        public async Task GivenPairId_WhenNotExists_ThenReturnNull()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);

            var result = await dao.GetByIdAsync(999);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GivenMultiplePairs_WhenGetAll_ThenReturnOrderedList()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var users = new List<User>
            {
                NewUser("u1@example.com", "User 1"),
                NewUser("u2@example.com", "User 2"),
                NewUser("u3@example.com", "User 3"),
                NewUser("u4@example.com", "User 4")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var pairs = new List<Pair>
            {
                NewPair(users[0].UserId, users[1].UserId, 2),
                NewPair(users[2].UserId, users[3].UserId, 1)
            };
            ctx.Pairs.AddRange(pairs);
            await ctx.SaveChangesAsync();

            var result = await dao.GetAllAsync();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Order);
            Assert.AreEqual(2, result[1].Order);
        }

        [TestMethod]
        public async Task GivenNoPairs_WhenGetAll_ThenReturnEmptyList()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);

            var result = await dao.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GivenPair_WhenDelete_ThenRemoveFromDatabase()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var user1 = NewUser("delete1@example.com", "Delete One");
            var user2 = NewUser("delete2@example.com", "Delete Two");
            ctx.Users.AddRange(user1, user2);
            await ctx.SaveChangesAsync();

            var pair = NewPair(user1.UserId, user2.UserId);
            ctx.Pairs.Add(pair);
            await ctx.SaveChangesAsync();
            var pairId = pair.PairId;

            await dao.DeleteAsync(pair);

            var result = await ctx.Pairs.FindAsync(pairId);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GivenPair_WhenUpdate_ThenSaveChanges()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var user1 = NewUser("update1@example.com", "Update One");
            var user2 = NewUser("update2@example.com", "Update Two");
            var user3 = NewUser("update3@example.com", "Update Three");
            ctx.Users.AddRange(user1, user2, user3);
            await ctx.SaveChangesAsync();

            var pair = NewPair(user1.UserId, user2.UserId);
            ctx.Pairs.Add(pair);
            await ctx.SaveChangesAsync();

            pair.UserId2 = user3.UserId;
            await dao.UpdateAsync(pair);

            var updated = await ctx.Pairs.FindAsync(pair.PairId);
            Assert.AreEqual(user3.UserId, updated.UserId2);
        }

        [TestMethod]
        public async Task GivenPairs_WhenGetMaxOrder_ThenReturnMaxValue()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var users = new List<User>
            {
                NewUser("max1@example.com", "Max 1"),
                NewUser("max2@example.com", "Max 2"),
                NewUser("max3@example.com", "Max 3"),
                NewUser("max4@example.com", "Max 4")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var pairs = new List<Pair>
            {
                NewPair(users[0].UserId, users[1].UserId, 5),
                NewPair(users[2].UserId, users[3].UserId, 10)
            };
            ctx.Pairs.AddRange(pairs);
            await ctx.SaveChangesAsync();

            var maxOrder = await dao.GetMaxOrderAsync();

            Assert.AreEqual(10, maxOrder);
        }

        [TestMethod]
        public async Task GivenNoPairs_WhenGetMaxOrder_ThenReturnZero()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);

            var maxOrder = await dao.GetMaxOrderAsync();

            Assert.AreEqual(0, maxOrder);
        }
    }

    [TestClass]
    public class CnPairTest
    {
        private SqliteConnection? _connection;
        private DbContextOptions<Context> _options;
        private Mock<CnUser> _mockCnUser;
        private Mock<CnSchedule> _mockCnSchedule;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = TestContextFactory.CreateOpenConnection();
            _options = TestContextFactory.CreateOptions(_connection);
            using var ctx = new Context(_options);
            ctx.Database.EnsureCreated();

            _mockCnUser = new Mock<CnUser>(null);
            _mockCnSchedule = new Mock<CnSchedule>(null, null);
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

        private User NewUser(string email, string name = "Test")
            => new User { Email = email, Name = name, PasswordHash = "hash" };

        [TestMethod]
        public async Task GivenTwoUsers_WhenCreatePair_ThenReturnNewPair()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            var user1 = NewUser("pair1@example.com", "Pair One");
            var user2 = NewUser("pair2@example.com", "Pair Two");
            ctx.Users.AddRange(user1, user2);
            await ctx.SaveChangesAsync();

            _mockCnSchedule.Setup(x => x.RegenerateSchedulesAsync()).Returns(Task.CompletedTask);

            var result = await cn.CreatePairAsync(user1.UserId, user2.UserId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.PairId > 0);
            Assert.AreEqual(user1.UserId, result.UserId1);
            Assert.AreEqual(user2.UserId, result.UserId2);
            Assert.AreEqual(1, result.Order);
            _mockCnSchedule.Verify(x => x.RegenerateSchedulesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GivenSameUserId_WhenCreatePair_ThenThrowInvalidOperationException()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await cn.CreatePairAsync(1, 1)
            );
        }

        [TestMethod]
        public async Task GivenSameUserId_WhenCreatePair_ThenThrowCorrectMessage()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await cn.CreatePairAsync(1, 1)
            );
            Assert.AreEqual("Não é possível criar um par com o mesmo usuário.", exception.Message);
        }

        [TestMethod]
        public async Task GivenUserAlreadyInPair_WhenCreatePair_ThenThrowInvalidOperationException()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            var users = new List<User>
            {
                NewUser("existing1@example.com", "Existing One"),
                NewUser("existing2@example.com", "Existing Two"),
                NewUser("new@example.com", "New User")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var existingPair = new Pair
            {
                UserId1 = users[0].UserId,
                UserId2 = users[1].UserId,
                Order = 1
            };
            ctx.Pairs.Add(existingPair);
            await ctx.SaveChangesAsync();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await cn.CreatePairAsync(users[0].UserId, users[2].UserId)
            );
        }

        [TestMethod]
        public async Task GivenMultiplePairs_WhenCreatePair_ThenIncrementOrder()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            var users = new List<User>
            {
                NewUser("order1@example.com", "Order 1"),
                NewUser("order2@example.com", "Order 2"),
                NewUser("order3@example.com", "Order 3"),
                NewUser("order4@example.com", "Order 4")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var existingPair = new Pair
            {
                UserId1 = users[0].UserId,
                UserId2 = users[1].UserId,
                Order = 5
            };
            ctx.Pairs.Add(existingPair);
            await ctx.SaveChangesAsync();

            _mockCnSchedule.Setup(x => x.RegenerateSchedulesAsync()).Returns(Task.CompletedTask);

            var result = await cn.CreatePairAsync(users[2].UserId, users[3].UserId);

            Assert.AreEqual(6, result.Order);
        }

        [TestMethod]
        public async Task GivenValidUpdate_WhenUpdatePair_ThenReturnUpdatedPair()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            var users = new List<User>
            {
                NewUser("upd1@example.com", "Update 1"),
                NewUser("upd2@example.com", "Update 2"),
                NewUser("upd3@example.com", "Update 3")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var pair = new Pair
            {
                UserId1 = users[0].UserId,
                UserId2 = users[1].UserId,
                Order = 1
            };
            ctx.Pairs.Add(pair);
            await ctx.SaveChangesAsync();

            _mockCnUser.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var result = await cn.UpdatePairAsync(pair.PairId, users[0].UserId, users[2].UserId);

            Assert.AreEqual(users[2].UserId, result.UserId2);
        }

        [TestMethod]
        public async Task GivenInvalidPairId_WhenUpdatePair_ThenThrowInvalidOperationException()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await cn.UpdatePairAsync(999, 1, 2)
            );
        }

        [TestMethod]
        public async Task GivenSameUserId_WhenUpdatePair_ThenThrowInvalidOperationException()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            var users = new List<User>
            {
                NewUser("same1@example.com", "Same 1"),
                NewUser("same2@example.com", "Same 2")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var pair = new Pair
            {
                UserId1 = users[0].UserId,
                UserId2 = users[1].UserId,
                Order = 1
            };
            ctx.Pairs.Add(pair);
            await ctx.SaveChangesAsync();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await cn.UpdatePairAsync(pair.PairId, users[0].UserId, users[0].UserId)
            );
        }

        [TestMethod]
        public async Task GivenNonExistentUser_WhenUpdatePair_ThenThrowInvalidOperationException()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            var users = new List<User>
            {
                NewUser("check1@example.com", "Check 1"),
                NewUser("check2@example.com", "Check 2")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var pair = new Pair
            {
                UserId1 = users[0].UserId,
                UserId2 = users[1].UserId,
                Order = 1
            };
            ctx.Pairs.Add(pair);
            await ctx.SaveChangesAsync();

            _mockCnUser.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await cn.UpdatePairAsync(pair.PairId, 999, 1000)
            );
        }

        [TestMethod]
        public async Task GivenValidPairId_WhenDelete_ThenRemovePair()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            var users = new List<User>
            {
                NewUser("del1@example.com", "Delete 1"),
                NewUser("del2@example.com", "Delete 2")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var pair = new Pair
            {
                UserId1 = users[0].UserId,
                UserId2 = users[1].UserId,
                Order = 1
            };
            ctx.Pairs.Add(pair);
            await ctx.SaveChangesAsync();
            var pairId = pair.PairId;

            await cn.DeletePairAsync(pairId);

            var result = await ctx.Pairs.FindAsync(pairId);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GivenInvalidPairId_WhenDelete_ThenThrowInvalidOperationException()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await cn.DeletePairAsync(999)
            );
        }

        [TestMethod]
        public async Task GivenPairs_WhenGetAll_ThenReturnAllPairs()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            var users = new List<User>
            {
                NewUser("all1@example.com", "All 1"),
                NewUser("all2@example.com", "All 2"),
                NewUser("all3@example.com", "All 3"),
                NewUser("all4@example.com", "All 4")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var pairs = new List<Pair>
            {
                new Pair { UserId1 = users[0].UserId, UserId2 = users[1].UserId, Order = 1 },
                new Pair { UserId1 = users[2].UserId, UserId2 = users[3].UserId, Order = 2 }
            };
            ctx.Pairs.AddRange(pairs);
            await ctx.SaveChangesAsync();

            var result = await cn.GetAllPairsAsync();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GivenValidPairId_WhenGetById_ThenReturnPair()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            var users = new List<User>
            {
                NewUser("getid1@example.com", "GetId 1"),
                NewUser("getid2@example.com", "GetId 2")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var pair = new Pair
            {
                UserId1 = users[0].UserId,
                UserId2 = users[1].UserId,
                Order = 1
            };
            ctx.Pairs.Add(pair);
            await ctx.SaveChangesAsync();

            var result = await cn.GetPairByIdAsync(pair.PairId);

            Assert.IsNotNull(result);
            Assert.AreEqual(pair.PairId, result.PairId);
        }

        [TestMethod]
        public async Task GivenInvalidPairId_WhenGetById_ThenThrowInvalidOperationException()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await cn.GetPairByIdAsync(999)
            );
        }

        [TestMethod]
        public async Task GivenUserId_WhenGetPairByUserId_ThenReturnPair()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            var users = new List<User>
            {
                NewUser("userid1@example.com", "UserId 1"),
                NewUser("userid2@example.com", "UserId 2")
            };
            ctx.Users.AddRange(users);
            await ctx.SaveChangesAsync();

            var pair = new Pair
            {
                UserId1 = users[0].UserId,
                UserId2 = users[1].UserId,
                Order = 1
            };
            ctx.Pairs.Add(pair);
            await ctx.SaveChangesAsync();

            var result = await cn.GetPairByUserId(users[0].UserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(pair.PairId, result.PairId);
        }

        [TestMethod]
        public async Task GivenInvalidUserId_WhenGetPairByUserId_ThenThrowInvalidOperationException()
        {
            using var ctx = new Context(_options);
            var dao = new DaoPair(ctx);
            var cn = new CnPair(dao, _mockCnUser.Object, _mockCnSchedule.Object);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await cn.GetPairByUserId(999)
            );
        }
    }

    [TestClass]
    public class PairModelTest
    {
        [TestMethod]
        public void GivenNewPair_WhenCreated_ThenPropertiesAreSet()
        {
            var pair = new Pair
            {
                UserId1 = 1,
                UserId2 = 2,
                Order = 5
            };

            Assert.AreEqual(1, pair.UserId1);
            Assert.AreEqual(2, pair.UserId2);
            Assert.AreEqual(5, pair.Order);
            Assert.IsNotNull(pair.Schedules);
        }

        [TestMethod]
        public void GivenNewPair_WhenCreated_ThenSchedulesCollectionIsInitialized()
        {
            var pair = new Pair();

            Assert.IsNotNull(pair.Schedules);
            Assert.AreEqual(0, pair.Schedules.Count);
        }
    }

    [TestClass]
    public class PairDtoTest
    {
        [TestMethod]
        public void GivenPairDto_WhenCreated_ThenPropertiesAreSet()
        {
            var user1 = new User { UserId = 1, Name = "User 1", Email = "user1@test.com", PasswordHash = "hash" };
            var user2 = new User { UserId = 2, Name = "User 2", Email = "user2@test.com", PasswordHash = "hash" };

            var pairDto = new PairDto
            {
                PairId = 1,
                UserId1 = 1,
                UserId2 = 2,
                User1 = user1,
                User2 = user2,
                NextScheduleId = 5,
                PurchasesQuantity = 10
            };

            Assert.AreEqual(1, pairDto.PairId);
            Assert.AreEqual(1, pairDto.UserId1);
            Assert.AreEqual(2, pairDto.UserId2);
            Assert.AreEqual("User 1", pairDto.User1.Name);
            Assert.AreEqual("User 2", pairDto.User2.Name);
            Assert.AreEqual(5, pairDto.NextScheduleId);
            Assert.AreEqual(10, pairDto.PurchasesQuantity);
            Assert.IsNotNull(pairDto.Purchases);
            Assert.IsNotNull(pairDto.Schedules);
        }
    }
}