using Microsoft.EntityFrameworkCore;
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
