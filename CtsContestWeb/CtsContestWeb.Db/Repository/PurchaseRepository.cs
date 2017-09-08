using System;
using CtsContestWeb.Db.Entities;
using System.Collections.Generic;

namespace CtsContestWeb.Db.DataAccess
{
    public class PurchaseRepository : IPurchaseRepository
    {
        public PurchaseRepository(ApplicationDbContext dbContext)
        {

        }

        public void Create(Purchase purchase)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Purchase> GetAllByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public void GiveAway(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
