using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateMahjongConnect.UI.WPF.Model
{
    public class GamerModel
    {
        public int Id { get; set; }
        public string? Pseudonyme { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public int? Score { get; set; }


        public GamerModel()
        {
            Id = 0;
            Pseudonyme = "";
            Age = 0;
            Email = "";
            Password = "";
            Score = 0;
        }

        public GamerModel(int id, string pseudonyme)
        {
            Id = id;
            Pseudonyme = pseudonyme;
        }

        public GamerModel(string pseudonyme, int age, string email, string password, int score)
        {
            Guard.Against.NullOrWhiteSpace(pseudonyme);
            Guard.Against.NullOrWhiteSpace(password);
            Guard.Against.NullOrWhiteSpace(email);
            Pseudonyme = pseudonyme;
            Age = age;
            Email = email;
            Password = password;
            Score = score;
        }
    }
}
