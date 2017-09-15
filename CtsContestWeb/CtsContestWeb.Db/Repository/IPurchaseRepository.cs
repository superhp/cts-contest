using System;
using CtsContestWeb.Db.Entities;
using System.Collections.Generic;

namespace CtsContestWeb.Db.DataAccess
{
    public interface IPurchaseRepository
    {
        IEnumerable<Purchase> GetAllByUserId(int userId);
        void Create(Purchase purchase);
        void GiveAway(Guid id);
    }
}