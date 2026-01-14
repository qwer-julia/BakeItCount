// TestContextFactory.cs
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using BakeItCountApi.Data;

namespace BakeItCountApi.Tests.Helpers
{
    /// <summary>
    /// Factory to create Contexts using SQLite in-memory.
    /// Must keep open the connection while the test needs to access DB.
    /// </summary>
    public static class TestContextFactory
    {
        public static SqliteConnection CreateOpenConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            return connection;
        }

        public static DbContextOptions<Context> CreateOptions(SqliteConnection connection)
        {
            return new DbContextOptionsBuilder<Context>()
                        .UseSqlite(connection)
                        .Options;
        }

        /// <summary>
        /// Creates a Context with EnsureCreated
        /// </summary>
        public static Context CreateContextAndEnsureCreated(SqliteConnection connection)
        {
            var options = CreateOptions(connection);
            var ctx = new Context(options);
            ctx.Database.EnsureCreated();
            return ctx;
        }
    }
}
