using UltimateMahjongConnect.Database.Net.Models;
using UltimateMahjongConnect.Service.DTO;

namespace UltimateMahjongConnect.Service.Interface
{
    public interface IGamerService
    {
        Task<int> AddGamerAsync(GamerDTO gamerDTO);
        Task<List<GamerEntity>> GetAllGamerAsync();
        Task<GamerEntity?> GetGamerByIdAsync(int Id);
    }
}