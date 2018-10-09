using System.Collections.Generic;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Duel
{
    public static class UserHandler
    {
        public static List<PlayerDto> WaitingPlayers = new List<PlayerDto>();
        public static List<DuelDto> ActiveDuels = new List<DuelDto>();
    }
}