using System.Collections.Generic;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Db.Repository
{
    public interface ICompetitionRepository
    {
        int CreateCompetition(CompetitionDto competition);
        void SetWinner(CompetitionDto competitionDto, PlayerDto winner);
        CompetitionSolution GetSolution(int competitionId, string userEmail);
        void UpsertSolution(CompetitionSolution solution);
        IEnumerable<CompetitionDto> GetWonCompetitionsByEmail(string userEmail);
        IEnumerable<CompetitionDto> GetCompetitionsByEmail(string email);
    }
}