using System;
using System.Collections.Generic;
using CtsContestWeb.Db.Entities;

namespace CtsContestWeb.Db.Repository
{
    public interface IPurchaseRepository
    {
        IEnumerable<Purchase> GetAllByUserEmail(string userEmail);
        Guid Create(string userEmail, int prizeId, int price);
        bool GiveAway(Guid id);
    }
}