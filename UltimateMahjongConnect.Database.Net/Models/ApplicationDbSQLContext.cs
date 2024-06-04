using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateMahjongConnect.Database.Net.Models
{
    public class ApplicationDbSQLContext : DbContext
    {
        public DbSet<Gamer> Gamers { get; set; } = null!;

        public ApplicationDbSQLContext(DbContextOptions<ApplicationDbSQLContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=UltimateMahjongConnectDB;User Id=sa;Password=Nap1815$; Trusted_Connection=True;");
        }
    }
}
