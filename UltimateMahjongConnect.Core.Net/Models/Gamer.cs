using System;

namespace UltimateMahjongConnect.Core.Net.Models
{
    public class Gamer
    {
        public string Pseudonyme { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public int Score { get; set; }
        private int _completedLevels;

        public Gamer(string pseudonyme, int age, string email)
        {
            Pseudonyme = pseudonyme;
            Age = age;
            Email = email;
            Score = 0; // Score initial
            _completedLevels = 0;
        }

        public void LevelCompleted()
        {
            _completedLevels++;

            if (_completedLevels == 5)
            {
                Score *= 2;
            }
        }
    }
}
