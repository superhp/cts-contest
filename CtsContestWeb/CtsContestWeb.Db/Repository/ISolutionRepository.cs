using System.Collections.Generic;
using CtsContestWeb.Db.Entities;

namespace CtsContestWeb.Db.Repository
{
    public interface ISolutionRepository
    {
        IEnumerable<int> GetTaskIdsByUserId(int userId);
        void Create(Solution solution);
    }
}