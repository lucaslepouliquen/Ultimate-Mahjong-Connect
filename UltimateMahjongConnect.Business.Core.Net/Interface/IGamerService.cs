using UltimateMahjongConnect.Database.Net.Models;

namespace UltimateMahjongConnect.Service.Interface
{
    public interface IGamerService
    {
        Task AddGamerAsync(GamerEntity gamer);
        Task<List<GamerEntity>> GetAllGamerAsync();
        Task<GamerEntity?> GetGamerByPseudonymeAsync(string pseudonyme);
    }
}