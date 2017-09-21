using System;
using System.Collections.Generic;
using CtsContestWeb.Db.DataAccess;
using CtsContestWeb.Db.Entities;

namespace CtsContestWeb.Db.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PurchaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Purchase purchase)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Purchase> GetAllByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public bool GiveAway(Guid id)
        {
            _dbContext.GivenPurchases.Add(new GivenPurchase
            {
                Created = DateTime.Now,
                GivenPurchaseId = id
            });
            return _dbContext.SaveChanges() == 1;
        }
    }
}
