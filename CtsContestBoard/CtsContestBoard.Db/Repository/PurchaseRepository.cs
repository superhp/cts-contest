using System;
using System.Collections.Generic;
using System.Linq;
using CtsContestBoard.Db.Entities;
using CtsContestBoard.Dto;

namespace CtsContestBoard.Db.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PurchaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IEnumerable<Purchase> GetAll()
        {
            return _dbContext.Purchases;
        }
    }
}
