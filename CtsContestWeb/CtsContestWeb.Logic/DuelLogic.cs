using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CtsContestWeb.Logic
{
    public class DuelLogic : IDuelLogic
    {
        private readonly IDuelRepository _duelRepository;
        private readonly IConfiguration _configuration;
        public DuelLogic(IDuelRepository duelRepository, IConfiguration configuration)
        {
            _duelRepository = duelRepository;
            _configuration = configuration;
        }

        public DuelDto CreateDuel(TaskDto task, PlayerDto firstPlayer, PlayerDto secondPlayer, int duration)
        {
            var duel = new DuelDto
            {
                Prize = task.Value,
                Players = new List<PlayerDto>
                {
                    firstPlayer,
                    secondPlayer
                },
                Task = task
            };
            duel.Players.ForEach(player =>
            {
                player.TotalWins = _duelRepository.GetWonDuelsByEmail(player.Email).Count();
                player.TotalLooses = _duelRepository.GetLostDuelsByEmail(player.Email).Count();
            });
            duel.Id = _duelRepository.CreateDuel(duel);
            duel.Duration = duration;

            return duel;
        }

        public int CalculateDuelDuration(string environment, int taskValue)
        {
            if (environment == "Development")
            {
                return 2;
            }
            switch (taskValue)
            {
                case 15:
                    return 15;
                case 20:
                    return 20;
                case 40:
                    return 30;
                default:
                    return _configuration.GetValue<int>("DefaultDuelDurationInMinutes");
            }
            
        }
    }
}
