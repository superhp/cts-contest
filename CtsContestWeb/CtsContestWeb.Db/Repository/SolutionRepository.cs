using System;
using CtsContestWeb.Db.Entities;
using System.Collections.Generic;

namespace CtsContestWeb.Db.DataAccess
{
    public class SolutionRepository : ISolutionRepository
    {
        public void Create(Solution solution)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetTaskIdsByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
