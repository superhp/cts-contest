﻿using System;
using System.Collections.Generic;
using CtsContestWeb.Db.DataAccess;
using CtsContestWeb.Db.Entities;
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

        public void Create(Purchase purchase)
        {
            _dbContext.Purchases.Add(purchase);
            _dbContext.SaveChanges();
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
    }
}
