using System;
using CtsContestWeb.Db.Entities;
using System.Collections.Generic;
using CtsContestWeb.Db.Repository;
using System.Linq;

namespace CtsContestWeb.Db.DataAccess
{
    public class SolutionRepository : ISolutionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SolutionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Solution solution)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetTaskIdsByUserEmail(string userEmail)
        {
            return _dbContext.Solutions.Where(x => x.UserEmail == userEmail).Select(x => x.TaskId);
        }
    }
}
