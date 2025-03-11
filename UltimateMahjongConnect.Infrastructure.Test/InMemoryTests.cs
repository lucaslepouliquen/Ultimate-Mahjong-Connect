using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UltimateMahjongConnect.Infrastructure.Models;
using UltimateMahjongConnect.Infrastructure.Persistence;
namespace UltimateMahjongConnect.Infrastructure.Test
{
    public class InMemoryTests
    {
        private static DbContextOptionsBuilder<ApplicationDbSQLContext> CreateInMemorySQLiteContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbSQLContext>();
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            builder.UseSqlite(_connection);
            return builder;
        }
        [Fact]
        public void CanInsertGamerIntoDatabase()
        {
            var builder = CreateInMemorySQLiteContext();
            using (var context = new ApplicationDbSQLContext(builder.Options))
            {
                context.Database.EnsureCreated();
                var gamer = new GamerEntity
                {
                    Pseudonyme = "TestGamer123",
                    Password = "SecurePassword123!",
                    Email = "testgamer@example.com",
                    BankDetails = "FR76 1234 5678 9012 3456 7890 123",
                    Age = 25,
                    Score = 1000
                };
                context.Gamers.Add(gamer);
                context.SaveChanges();
                Assert.NotEqual(0, gamer.Id);
            }
        }
        [Fact]
        public void CanUpdateGamerFromDatabase()
        {
            DbContextOptionsBuilder<ApplicationDbSQLContext> builder = CreateInMemorySQLiteContext();
            using (var context = new ApplicationDbSQLContext(builder.Options))
            {
                context.Database.EnsureCreated();
                var gamer = context.Gamers.Find(1);
                gamer.Pseudonyme = "UpdatedGamer123";
                context.SaveChanges();
                Assert.Equal(context.Gamers.Find(1).Pseudonyme, "UpdatedGamer123");
            }
        }
        [Fact]
        public void CanDeleteGamerFromDatabase()
        {
            var builder = CreateInMemorySQLiteContext();
            using (var context = new ApplicationDbSQLContext(builder.Options))
            {
                context.Database.EnsureCreated();
                context.Remove(context.Gamers.Find(1));
                context.SaveChanges();
                var gamer = context.Gamers.Find(1);
                Assert.Null(gamer);
            }
        }
    }
}