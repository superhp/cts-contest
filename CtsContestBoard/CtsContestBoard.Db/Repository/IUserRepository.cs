using CtsContestBoard.Db.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtsContestBoard.Db.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
    }
}
