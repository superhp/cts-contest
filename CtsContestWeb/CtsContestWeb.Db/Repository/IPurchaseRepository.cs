using System;
using System.Collections.Generic;
using CtsContestWeb.Db.Entities;

namespace CtsContestWeb.Db.Repository
{
    public interface IPurchaseRepository
    {
        IEnumerable<Purchase> GetAllByUserEmail(string userEmail);
        void Create(Purchase purchase);
        bool GiveAway(Guid id);
    }
}