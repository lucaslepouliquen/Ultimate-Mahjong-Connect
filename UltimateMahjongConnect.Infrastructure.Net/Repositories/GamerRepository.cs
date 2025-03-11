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

        public async Task<List<GamerDTO>> GetAllGamerAsync()
        {
            var entities = await _context.Gamers.ToListAsync();
            return _mapper.Map<List<GamerDTO>>(entities);
        }

        public async Task<GamerDTO?> GetGamerByIdAsync(int id)
        {
            var entity = await _context.Gamers.FirstOrDefaultAsync(g => g.Id == id);
            return entity != null ? _mapper.Map<GamerDTO>(entity) : null;
        }

        Task<List<Gamer>> IGamerRepository.GetAllGamerAsync()
        {
            throw new NotImplementedException();
        }

        Task<Gamer?> IGamerRepository.GetGamerByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }
    }
}