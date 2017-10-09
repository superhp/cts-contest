using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CtsContestBoard.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace CtsContestBoard.Db.Repository
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

        public IEnumerable<Solution> GetAll()
        {
            return _dbContext.Solutions.Include(s => s.User);
        }
    }
}
