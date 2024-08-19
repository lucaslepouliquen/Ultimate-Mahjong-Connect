using UltimateMahjongConnect.Database.Net.Models;

namespace UltimateMahjongConnect.Service.Interface
{
    public interface IGamerService
    {
        Task AddGamerAsync(Gamer gamer);
        Task<List<Gamer>> GetAllGamerAsync();
        Task<Gamer?> GetGamerByPseudonymeAsync(string pseudonyme);
    }
}