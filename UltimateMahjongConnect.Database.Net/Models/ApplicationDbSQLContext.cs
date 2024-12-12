using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UltimateMahjongConnect.Database.Net.Models
{
    public class ApplicationDbSQLContext : DbContext
    {
        public DbSet<GamerEntity> Gamers { get; set; } = null!;
        public ApplicationDbSQLContext()
        {
            
        }
        public ApplicationDbSQLContext(DbContextOptions<ApplicationDbSQLContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var gamerList = new GamerEntity[]{
                 new GamerEntity { Id = 1, Pseudonyme = "TestGamer123", Password = "SecurePassword123!",
                    Email = "testgamer@example.com", BankDetails = "FR76 1234 5678 9012 3456 7890 123", Age = 25, Score = 1000},
                new GamerEntity { Id = 2, Pseudonyme = "DarkZelios", Password = "DarkZelios123!",
                    Email = "darkzelios@example.com", BankDetails = "FR76 7890 5678 9012 3456 1234 123", Age = 18, Score = 10000}
            };

            modelBuilder.Entity<GamerEntity>().HasData(gamerList);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
#if NET8_0 && WINDOWS
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
#endif
        }

    }
}
