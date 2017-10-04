using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Db.Repository
{
    public interface IPurchaseRepository
    {
        Task<IEnumerable<Purchase>> GetAllByUserEmail(string userEmail);
        Guid Create(string userEmail, int prizeId, int price);
        bool GiveAway(Guid id);
        PurchaseDto GetPurchaseByPurchaseGuid(Guid id);
        Task<IEnumerable<Purchase>> GetAll();
    }
}