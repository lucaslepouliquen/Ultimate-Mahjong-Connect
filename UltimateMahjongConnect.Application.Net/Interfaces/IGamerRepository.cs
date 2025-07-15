using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Interface
{
    public interface IGamerRepository
    {
        Task<int> CreateGamerAsync(Gamer gamer);
        Task<List<Gamer>> GetAllGamerAsync();
        Task<Gamer?> GetGamerByIdAsync(int Id);
        Task<Gamer?> GetGamerByPseudonymeAsync(string pseudonyme);
        Task<Gamer?> GetGamerByEmailAsync(string email);
        Task<Gamer?> UpdatedGamerAsync(Gamer gamer);
        Task DeleteGamerAsync(Gamer gamer);
    }
}