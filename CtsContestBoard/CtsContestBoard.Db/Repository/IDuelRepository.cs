using System.Collections.Generic;
using CtsContestBoard.Dto;
using CtsContestWeb.Db.Entities;

namespace CtsContestBoard.Db.Repository
{
    public interface IDuelRepository
    {
        DuelSolution GetSolution(int competitionId, string userEmail);
        IEnumerable<DuelDto> GetWonDuelsByEmail(string userEmail);
        IEnumerable<DuelDto> GetDuelsByEmail(string email);
        IEnumerable<DuelDto> GetLostDuelsByEmail(string playerEmail);
        IEnumerable<DuelDto> GetAllDuelsWithWinner();
    }
}