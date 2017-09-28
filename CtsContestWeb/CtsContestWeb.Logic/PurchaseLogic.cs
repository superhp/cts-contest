using CtsContestWeb.Db.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CtsContestWeb.Logic
{
    public class PurchaseLogic : IPurchaseLogic
    {
        private readonly IPurchaseRepository _purRep;
        private readonly IBalanceLogic _balanceLogic;
        
        public PurchaseLogic(IPurchaseRepository purchaseRepository, IBalanceLogic balanceLogic)
        {
            _purRep = purchaseRepository;
            _balanceLogic = balanceLogic;
        }

        public bool CheckIfUserCanBuy(string userEmail, int prizeId)
        {
            var purchases = _purRep.GetAllByUserEmail(userEmail);
            return purchases.Any(x => x.PrizeId == prizeId);
        }
    }
}
