using CtsContestWeb.Db.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<bool> CheckIfUserCanBuy(string userEmail, int prizeId)
        {
            var purchases = await _purRep.GetAllByUserEmail(userEmail);
            if (purchases.Any(x => x.PrizeId == prizeId)) return false;
            return await _balanceLogic.IsBalanceEnough(userEmail, prizeId);
        }
    }
}
