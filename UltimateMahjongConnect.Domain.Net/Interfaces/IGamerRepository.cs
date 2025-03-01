using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Domain.Interfaces
{
    interface IGamerRepository
    {
        Task AddAsync(Gamer gamer);
        Task<List<Gamer>> GetAllAsync();
        Task<Gamer?> GetByIdAsync(int id);
    }
}
