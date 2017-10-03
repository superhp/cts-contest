using System;
using System.Collections.Generic;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Dto;
using System.Linq;

namespace CtsContestWeb.Db.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PurchaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Create(string userEmail, int prizeId, int price)
        {
            var purchase = new Purchase
            {
                UserEmail = userEmail,
                PrizeId = prizeId,
                Created = DateTime.Now,
                PurchaseId = Guid.NewGuid(),
                Cost = price
            };
            _dbContext.Purchases.Add(purchase);
            _dbContext.SaveChanges();

            return purchase.PurchaseId;
        }

        public IEnumerable<Purchase> GetAllByUserEmail(string userEmail)
        {
            return _dbContext.Purchases.Where(x => x.UserEmail == userEmail);
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

        public PurchaseDto GetPurchaseByPurchaseGuid(Guid id)
        {
            var purchase = _dbContext.Purchases.Find(id);
            purchase.GivenPurchase = _dbContext.GivenPurchases.Find(id);

            return new PurchaseDto
            {
                PurchaseId = purchase.PurchaseId,
                UserEmail = purchase.UserEmail,
                PrizeId = purchase.PrizeId,
                IsGivenAway = purchase.GivenPurchase != null
            };
        }
    }
}
