using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Duel
{
    public static class UserHandler
    {
        public static List<PlayerDto> WaitingPlayers = new List<PlayerDto>();
        public static List<DuelDto> ActiveDuels = new List<DuelDto>();

        public static bool IsPlayerInDuel(string email) => ActiveDuels.SelectMany(d => d.Players).Any(p => p.Email.Equals(email));
    }
}