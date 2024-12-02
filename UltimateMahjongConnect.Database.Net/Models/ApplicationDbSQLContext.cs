using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UltimateMahjongConnect.Database.Net.Models
{
    public class ApplicationDbSQLContext : DbContext
    {
        public DbSet<GamerEntity> Gamers { get; set; } = null!;

        public ApplicationDbSQLContext(DbContextOptions<ApplicationDbSQLContext> options)
            : base(options)
        {
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
