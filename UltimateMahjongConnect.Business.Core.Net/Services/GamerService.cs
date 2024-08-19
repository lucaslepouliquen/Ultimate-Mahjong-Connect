using Microsoft.EntityFrameworkCore;
using UltimateMahjongConnect.Database.Net.Models;
using UltimateMahjongConnect.Service.Interface;

namespace UltimateMahjongConnect.Service.Services
{
    public class GamerService : IGamerService
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

        public async Task<List<Gamer>> GetAllGamerAsync()
        {
            return await _context.Gamers.ToListAsync();
        }

        public async Task<Gamer?> GetGamerByPseudonymeAsync(string pseudonyme)
        {
            return await _context.Gamers.FirstOrDefaultAsync(g => g.Pseudonyme == pseudonyme);
        }
    }
}
