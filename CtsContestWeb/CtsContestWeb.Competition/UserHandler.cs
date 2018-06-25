using System.Collections.Generic;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Competition
{
    public static class UserHandler
    {
        public static List<PlayerDto> WaitingPlayers = new List<PlayerDto>();
        public static List<CompetitionDto> ActiveCompetitions = new List<CompetitionDto>();
    }
}