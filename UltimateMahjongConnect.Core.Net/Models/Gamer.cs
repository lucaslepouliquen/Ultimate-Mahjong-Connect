using System;
using UltimateMahjongConnect.Business.Interfaces;

namespace UltimateMahjongConnect.Core.Net.Models
{
    public class Gamer : IGamer
    {
        private string _pseudonyme;
        private int _age;
        private string _email;
        private int _score;
        private string _password;
        private int _completedLevels;

        public Gamer()
        {
            _pseudonyme = "test";
            _age = 18;
            _email = "test@gmail.com";
            _password = "test";
        }
        public Gamer(string pseudonyme, int age, string email)
        {
            _pseudonyme = pseudonyme;
            _age = age;
            _email = email;
            _score = 0; 
            _completedLevels = 0;
        }

        public void AddScore(int score)
        {
            Score += score;
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
