using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UltimateMahjongConnect.Infrastructure.Models
{
    public class ApplicationDbSQLContextFactory : IDesignTimeDbContextFactory<ApplicationDbSQLContext>
    {
        public ApplicationDbSQLContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbSQLContext>();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            return new ApplicationDbSQLContext(optionsBuilder.Options);
        }
    }
}