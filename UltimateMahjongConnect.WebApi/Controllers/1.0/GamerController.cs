using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UltimateMahjongConnect.Application.Interface;
using UltimateMahjongConnect.Domain.Models;
using UltimateMahjongConnect.Infrastructure.Models;
using UltimateMahjongConnect.Infrastructure.Persistence;

namespace UltimateMahjongConnect.Infrastructure.Repositories
{
    public class GamerRepository : IGamerRepository
    {
        private readonly ApplicationDbSQLContext _context;
        private readonly IMapper _mapper;

        public GamerRepository(ApplicationDbSQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddGamerAsync(Gamer gamer)
        {
            try
            {
                var entity = _mapper.Map<GamerEntity>(gamer);
                _context.Gamers.Add(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Gamer>> GetAllGamerAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Gamer>> GetAllGamersAsync()
        {
            var entities = await _context.Gamers.ToListAsync();
            return _mapper.Map<List<Gamer>>(entities);
        }

        public async Task<Gamer?> GetGamerByIdAsync(int id)
        {
            var entity = await _context.Gamers.FirstOrDefaultAsync(g => g.Id == id);
            return entity != null ? _mapper.Map<Gamer>(entity) : null;
        }
    }
}