using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UltimateMahjongConnect.Database.Net.Models;

namespace UltimateMahjongConnect.Database.Test
{
    public class InMemoryTests
    {
        [Fact]
        public void CanInsertGamerIntoDatabse()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbSQLContext>();
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            builder.UseSqlite(_connection);
            using (var context = new ApplicationDbSQLContext(builder.Options))
            {
                context.Database.EnsureCreated();
                var gamer = new GamerEntity
                {
                    Id = 1,
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
    }
}