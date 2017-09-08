using CtsContestWeb.Db.Entities;
using System.Collections.Generic;

namespace CtsContestWeb.Db.DataAccess
{
    public interface ISolutionRepository
    {
        IEnumerable<int> GetTaskIdsByUserId(int userId);
        void Create(Solution solution);
    }
}