using System;

namespace MahjongConnect.Logic
{
    public class Client
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public int Score { get; set; }
        private int _completedLevels;

        public Client(string firstName, string lastName, int age, string email)
        {
            FirstName = firstName;
            LastName = lastName;
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
