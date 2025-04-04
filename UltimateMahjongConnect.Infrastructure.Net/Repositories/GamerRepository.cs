using UltimateMahjongConnect.Infrastructure.Models;
using UltimateMahjongConnect.Application.Interface;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Domain.Models;
using UltimateMahjongConnect.Infrastructure.Persistence;
using AutoMapper;

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

        public async Task DeleteGamerAsync(Gamer gamer)
        {
            try
            {
                var entity = _mapper.Map<GamerEntity>(gamer);
                _context.Entry(entity).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw; 
            }
        }

        public async Task<List<Gamer>> GetAllGamerAsync()
        {
            try
            {
                var entities = await _context.Gamers.ToListAsync();
                return _mapper.Map<List<Gamer>>(entities);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Gamer?> GetGamerByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Gamers.FirstOrDefaultAsync(g => g.Id == id);
                return entity != null ? _mapper.Map<Gamer>(entity) : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Gamer?> UpdatedGamerAsync(Gamer gamer)
        {
            try
            {
                var entity = _mapper.Map<GamerEntity>(gamer);
                _context.Gamers.Update(entity);
                await _context.SaveChangesAsync();
                return _mapper.Map<Gamer>(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}