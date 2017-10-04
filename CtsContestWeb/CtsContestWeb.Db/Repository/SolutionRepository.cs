using System;
using System.Collections.Generic;
using CtsContestWeb.Db.Repository;
using System.Linq;
using CtsContestWeb.Db.Entities;

namespace CtsContestWeb.Db.Repository
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
            _dbContext.Add(solution);
            _dbContext.SaveChanges();
        }

        public IEnumerable<int> GetTaskIdsByUserEmail(string userEmail)
        {
            return _dbContext.Solutions.Where(x => x.UserEmail == userEmail).Select(x => x.TaskId);
        }

        public IEnumerable<Solution> GetSolutionsByUserEmail(string userEmail)
        {
            return _dbContext.Solutions.Where(x => x.UserEmail == userEmail);
        }
    }
}
