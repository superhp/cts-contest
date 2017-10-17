using System;
using System.Collections.Generic;
using CtsContestWeb.Db.Repository;
using System.Linq;
using CtsContestWeb.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace CtsContestWeb.Db.Repository
{
    public class SolutionRepository : ISolutionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SolutionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Upsert(Solution solution)
        {
            _dbContext.Entry(solution).State = solution.SolutionId == 0 ?
                EntityState.Added :
                EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public IEnumerable<int> GetSolvedTasksIdsByUserEmail(string userEmail)
        {
            return _dbContext.Solutions.Where(x => x.UserEmail == userEmail && x.IsCorrect).Select(x => x.TaskId);
        }

        public IEnumerable<Solution> GetSolutionsByUserEmail(string userEmail)
        {
            return _dbContext.Solutions.Where(x => x.UserEmail == userEmail);
        }

        public Solution GetSolution(string email, int taskId)
        {
            return _dbContext.Solutions.FirstOrDefault(s => s.UserEmail.Equals(email) && s.TaskId == taskId);
        }
    }
}
