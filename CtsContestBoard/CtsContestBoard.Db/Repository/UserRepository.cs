using CtsContestBoard.Db.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtsContestBoard.Db.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<User> GetAll()
        {
            return _dbContext.Users;
        }
    }
}
