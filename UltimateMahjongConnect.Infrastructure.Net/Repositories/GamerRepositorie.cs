using Microsoft.EntityFrameworkCore;
using UltimateMahjongConnect.Infrastructure.Models;

namespace UltimateMahjongConnect.Infrastructure.Repositories
{
    public class GamerRepositorie : IGamerService
    {
        private readonly ApplicationDbSQLContext _context;
        public GamerRepositorie(ApplicationDbSQLContext context)
        {
            _context = context;
        }

        public async Task<int> AddGamerAsync(GamerEntity gamer)
        {
            try
            {
                _context.Gamers.Add(gamer);
                await _context.SaveChangesAsync();
                return gamer.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GamerEntity>> GetAllGamerAsync()
        {
            return await _context.Gamers.ToListAsync();
        }

        public async Task<GamerEntity?> GetGamerByIdAsync(int Id)
        {
            return await _context.Gamers.FirstOrDefaultAsync(g => g.Id == Id);
        }
    }
}
