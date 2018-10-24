using System.Collections.Generic;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Db.Repository
{
    public interface IDuelRepository
    {
        int CreateDuel(DuelDto competition);
        void SetWinner(DuelDto competitionDto, PlayerDto winner);
        DuelSolution GetSolution(int competitionId, string userEmail);
        void UpsertSolution(DuelSolution solution);
        IEnumerable<DuelDto> GetWonDuelsByEmail(string userEmail);
        IEnumerable<DuelDto> GetDuelsByEmail(string email);
        IEnumerable<DuelDto> GetLostDuelsByEmail(string playerEmail);
    }
}