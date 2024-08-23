using Microsoft.EntityFrameworkCore;

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
                optionsBuilder.UseSqlServer("Server=localhost,1433;Database=UltimateMahjongConnectDB;User Id=sa;Password=Nap1815$; TrustServerCertificate=True;");
            }
        }
    }
}
