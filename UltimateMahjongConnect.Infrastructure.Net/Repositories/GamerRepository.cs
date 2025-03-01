using UltimateMahjongConnect.Infrastructure.Models;
using UltimateMahjongConnect.Domain.Models;
using UltimateMahjongConnect.Domain.Interfaces; 

namespace UltimateMahjongConnect.Infrastructure.Repositories
{
    public class GamerRepository : IGamerRepository
    {
        private readonly ApplicationDbSQLContext _context;
        public GamerRepository(ApplicationDbSQLContext context)
        {
            _context = context;
        }

        public async Task<int> AddGamerAsync(Gamer gamer)
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

        public async Task<List<Gamer>> GetAllGamerAsync()
        {
            return await _context.Gamers.ToListAsync();
        }

        public async Task<Gamer?> GetGamerByIdAsync(int Id)
        {
            return await _context.Gamers.FirstOrDefaultAsync(g => g.Id == Id);
        }
    }
}
