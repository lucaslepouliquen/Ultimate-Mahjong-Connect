using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateMahjongConnect.UI.WPF.Model;

namespace UltimateMahjongConnect.UI.WPF.Data
{
    public class GamerDataProvider : IGamerDataProvider
    {
        public Task<IEnumerable<GamerModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
