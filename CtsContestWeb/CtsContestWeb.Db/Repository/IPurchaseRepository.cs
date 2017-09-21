using System;
using System.Collections.Generic;
using CtsContestWeb.Db.Entities;

namespace CtsContestWeb.Db.Repository
{
    public interface IPurchaseRepository
    {
        IEnumerable<Purchase> GetAllByUserId(int userId);
        void Create(Purchase purchase);
        bool GiveAway(Guid id);
    }
}