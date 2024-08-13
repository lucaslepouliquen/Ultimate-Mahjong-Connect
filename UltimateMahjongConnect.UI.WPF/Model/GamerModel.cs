using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateMahjongConnect.UI.WPF.Model
{
    public class GamerModel
    {
        public string? Pseudonyme { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }

        public GamerModel()
        {
            Pseudonyme = "";
            Age = 0;
            Email = "";
            Password = "";  
        }

        public GamerModel(string pseudonyme, int age, string email, string password)
        {
            Pseudonyme = pseudonyme;
            Age = age;
            Email = email;
            Password = password;
        }
    }
}
