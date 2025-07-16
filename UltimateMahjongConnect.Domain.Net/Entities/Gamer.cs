using UltimateMahjongConnect.Domain.Interfaces;

namespace UltimateMahjongConnect.Domain.Models
{
    public class Gamer : IGamer
    {
        private int _completedLevels;
        
        public int Id { get; set; }
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Pseudonyme { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int Score { get; set; }
        public Gamer()
        {
            Pseudonyme = "test";
            Age = 18;
            Email = "test@gmail.com";
            PasswordHash = "test";
            Score = 0;
            _completedLevels = 0;
        }
        
        public Gamer(string pseudonyme, int age, string email)
        {
            Pseudonyme = pseudonyme;
            Age = age;
            Email = email;
            Score = 0; 
            _completedLevels = 0;
        }

        public void AddScore(int points)
        {
            Score += points;
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
