using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Interface
{
    public interface IGamerService
    {
        Task<int> AddGamerAsync(GamerDTO gamerDTO);
        Task<List<GamerEntity>> GetAllGamerAsync();
        Task<GamerEntity?> GetGamerByIdAsync(int Id);
    }
}