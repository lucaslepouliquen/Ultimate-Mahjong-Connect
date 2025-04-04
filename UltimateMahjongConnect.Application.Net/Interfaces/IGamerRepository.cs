using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Interface
{
    public interface IGamerRepository
    {
        Task<int> AddGamerAsync(Gamer gamer);
        Task<List<Gamer>> GetAllGamerAsync();
        Task<Gamer?> GetGamerByIdAsync(int Id);
        Task<Gamer?> UpdatedGamerAsync(Gamer gamer);
        Task DeleteGamerAsync(Gamer gamer);
    }
}