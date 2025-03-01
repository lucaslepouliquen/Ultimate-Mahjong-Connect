using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UltimateMahjongConnect.Database.Net.Models;
using UltimateMahjongConnect.Service.DTO;
using UltimateMahjongConnect.Service.Interface;

namespace UltimateMahjongConnect.Service.Services
{
    public class GamerService : IGamerService
    {
        private readonly ApplicationDbSQLContext _context;
        private IMapper _mapper;
        public GamerService(ApplicationDbSQLContext context)
        {
            _context = context;
        }

        public async Task<int> AddGamerAsync(GamerDTO gamer)
        {
            try
            {
                var gamerEntity = _mapper.Map<GamerEntity>(gamer);
                _context.Gamers.Add(gamerEntity);
                await _context.SaveChangesAsync();
                return gamer.Id;
            }
            catch (System.Exception)
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
