using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateMahjongConnect.Application.Interface;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Domain.Models;
using AutoMapper;

namespace UltimateMahjongConnect.Application.Services
{
    public class GamerService
    {
        private readonly IGamerRepository _gamerRepository;
        private readonly IMapper _mapper;

        public GamerService(IGamerRepository gamerRepository, IMapper mapper)
        {
            _gamerRepository = gamerRepository;
            _mapper = mapper;
        }

        public async Task<List<GamerDTO>> GetAllGamerAsync()
        {
            try
            {
                var gamers = await _gamerRepository.GetAllGamerAsync();
                return _mapper.Map<List<GamerDTO>>(gamers);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Une erreur est survenue lors de la récupération des joueurs", ex);
            }
        }

        public async Task<GamerDTO?> GetGamerByPseudonymeAsync(string pseudonyme)
        {
            if (string.IsNullOrWhiteSpace(pseudonyme))
            {
                throw new ArgumentException("Le pseudonyme ne peut pas être vide", nameof(pseudonyme));
            }

            try
            {
                var gamers = await _gamerRepository.GetAllGamerAsync();
                var gamer = gamers.FirstOrDefault(g => g.Pseudonyme.Equals(pseudonyme, StringComparison.OrdinalIgnoreCase));

                return gamer != null ? _mapper.Map<GamerDTO>(gamer) : null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Une erreur est survenue lors de la récupération du joueur avec le pseudonyme '{pseudonyme}'", ex);
            }
        }

        public async Task<int> CreateGamerAsync(GamerDTO gamerDTO)
        {
            if (gamerDTO == null)
            {
                throw new ArgumentNullException(nameof(gamerDTO));
            }

            if (string.IsNullOrWhiteSpace(gamerDTO.Pseudonyme))
            {
                throw new ArgumentException("Le pseudonyme ne peut pas être vide", nameof(gamerDTO));
            }

            try
            {
                var gamer = _mapper.Map<Gamer>(gamerDTO);
                return await _gamerRepository.AddGamerAsync(gamer);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Une erreur est survenue lors de la création du joueur", ex);
            }
        }
    }
}