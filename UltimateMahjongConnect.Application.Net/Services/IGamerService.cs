using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Services
{
    public interface IGamerService
    {
        Task<List<GamerDTO>> GetAllGamerAsync();
        Task<GamerDTO?> GetGamerByPseudonymeAsync(string pseudonyme);
        Task<GamerDTO?> GetGamerByIdAsync(int id);
        Task<int> CreateGamerAsync(GamerDTO gamerDTO);
        Task<GamerDTO> UpdateGamerAsync(GamerDTO gamerDTO);
        Task DeleteGamerAsync(GamerDTO gamerDTO);

    }
}