﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CtsContestBoard.Db.Entities;

namespace CtsContestBoard.Db.Repository
{
    public interface ISolutionRepository
    {
        IEnumerable<int> GetTaskIdsByUserEmail(string userEmail);
        void Create(Solution solution);
        IEnumerable<Solution> GetAll();
    }
}