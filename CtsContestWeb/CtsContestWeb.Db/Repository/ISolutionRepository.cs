using System.Collections.Generic;
using CtsContestWeb.Db.Entities;

namespace CtsContestWeb.Db.Repository
{
    public interface ISolutionRepository
    {
        IEnumerable<int> GetSolvedTasksIdsByUserEmail(string userEmail);
        void Upsert(Solution solution);
        IEnumerable<Solution> GetSolutionsByUserEmail(string userEmail);
        Solution GetSolution(string email, int taskId);
    }
}