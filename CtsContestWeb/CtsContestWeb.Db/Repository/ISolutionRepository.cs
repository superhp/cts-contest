using System.Collections.Generic;
using CtsContestWeb.Db.Entities;

namespace CtsContestWeb.Db.Repository
{
    public interface ISolutionRepository
    {
        IEnumerable<int> GetTaskIdsByUserEmail(string userEmail);
        void Create(Solution solution);
    }
}