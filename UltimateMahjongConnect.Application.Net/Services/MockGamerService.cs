using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Application.Interface;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Services
{
    public class MockGamerService : IGamerService
    {
        private readonly List<GamerEntity> _gamers;

        public MockGamerService()
        {
            _gamers = new List<GamerEntity>
            {
                new GamerEntity { Id = 1, Pseudonyme = "Gamer1", Password = "password1", Email = "gamer1@mail.com", BankDetails = "1234", Age = 25, Score = 100 },
                new GamerEntity { Id = 2, Pseudonyme = "Gamer2", Password = "password2", Email = "gamer2@mail.com", BankDetails = "5678", Age = 30, Score = 200 }
            };
        }

        public Task AddGamerAsync(GamerEntity gamer)
        {
            gamer.Id = _gamers.Max(g => g.Id) + 1;
            _gamers.Add(gamer);
            return Task.CompletedTask;
        }

        public Task<List<GamerEntity>> GetAllGamerAsync()
        {
            return Task.FromResult(_gamers);
        }

        public Task<List<GamerEntity>> GetAllGamersAsync()
        {
            return Task.FromResult(_gamers);
        }

        public Task<GamerEntity?> GetGamerByPseudonymAsync(string pseudonym)
        {
            var gamer = _gamers.FirstOrDefault(g => g.Pseudonyme == pseudonym);
            return Task.FromResult(gamer);
        }

        public Task<GamerEntity?> GetGamerByPseudonymeAsync(string pseudonyme)
        {
            throw new NotImplementedException();
        }

        Task<int> IGamerService.AddGamerAsync(GamerDTO gamerDTO)
        {
            throw new NotImplementedException();
        }

        Task<GamerEntity?> IGamerService.GetGamerByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
