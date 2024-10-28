﻿using System;
using UltimateMahjongConnect.Business.Interfaces;

namespace UltimateMahjongConnect.Core.Net.Models
{
    public class Gamer : IGamer
    {
        private readonly string _pseudonyme;
        private readonly int _age;
        private readonly string _email;
        private readonly string _password;
        private int _completedLevels;
        private int score { get; set; }
        public int Score => score;
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
            score = 0; 
            _completedLevels = 0;
        }

        public void AddScore(int score)
        {
            score += score;
        }

        public void LevelCompleted()
        {
            _completedLevels++;

            if (_completedLevels == 5)
            {
                score *= 2;
            }
        }
    }
}
