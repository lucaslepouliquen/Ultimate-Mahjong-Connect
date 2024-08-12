using UltimateMahjongConnect.Database.Net.Models;

namespace UltimateMahjongConnect.Service.Services
{
    public class GamerService
    {
        private readonly ApplicationDbSQLContext _context;

        public GamerService(ApplicationDbSQLContext context)
        {
            _context = context;
        }

        public async Task AddGamerAsync(Gamer gamer)
        {
            _context.Gamers.Add(gamer);
            await _context.SaveChangesAsync();
        }
    }
}
