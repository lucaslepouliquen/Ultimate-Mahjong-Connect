using Microsoft.EntityFrameworkCore.Design;

namespace UltimateMahjongConnect.Database.Net.Models
{
    public class ApplicationDbSQLContextFactory : IDesignTimeDbContextFactory<ApplicationDbSQLContext>
    {
        public ApplicationDbSQLContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbSQLContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=UltimateMahjongConnectDB;User Id=sa;Password=Nap1815$; TrustServerCertificate=True;");

            return new ApplicationDbSQLContext(optionsBuilder.Options);
        }
    }
}