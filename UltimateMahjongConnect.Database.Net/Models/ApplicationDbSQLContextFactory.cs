using Microsoft.EntityFrameworkCore.Design;

namespace UltimateMahjongConnect.Database.Net.Models
{
    public class ApplicationDbSQLContextFactory : IDesignTimeDbContextFactory<ApplicationDbSQLContext>
    {
        public ApplicationDbSQLContext CreateDbContext(string[] args)
        {
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var connectionString = $"Server=localhost,1433;Database=UltimateMahjongConnectDB;User Id=sa;Password={password};TrustServerCertificate=True;";
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbSQLContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbSQLContext(optionsBuilder.Options);
        }
    }
}