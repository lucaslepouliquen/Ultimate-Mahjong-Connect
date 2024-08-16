using UltimateMahjongConnect.UI.WPF.Model;

namespace UltimateMahjongConnect.UI.WPF.Data
{
    public interface IGamerDataProvider
    {
        Task<IEnumerable<GamerModel>> GetAllAsync();
    }
}
