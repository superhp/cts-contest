using CtsContestWeb.Communication;
using CtsContestWeb.Db.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CtsContestWeb.Logic
{
    public class BalanceLogic : IBalanceLogic
    {
        private readonly ISolutionRepository _solRep;
        private readonly IPurchaseRepository _purRep;
        private readonly ITaskManager _taskManager;
        private readonly IPrizeManager _prizeManager;

        public BalanceLogic(ISolutionRepository solRep, ITaskManager taskManager, IPurchaseRepository purRep, IPrizeManager prizeManager)
        {
            _solRep = solRep;
            _taskManager = taskManager;
            _purRep = purRep;
            _prizeManager = prizeManager;
        }

        public async Task<bool> IsBalanceEnough(string userEmail, int prizeId)
        {
            var prize = await _prizeManager.GetPrizeById(prizeId);
            var price = prize.Price;
            var balance = GetCurrentBalance(userEmail); 
            return balance >= price;
        }

        public int GetCurrentBalance(string userEmail)
        {
            return GetTotalEarnedMoney(userEmail) - GetTotalSpentMoney(userEmail);
        }

        private int GetTotalEarnedMoney(string userEmail)
        {
            var solutions = _solRep.GetSolutionsByUserEmail(userEmail);
            var sum = solutions.Where(s => s.IsCorrect).Select(x => x.Score).DefaultIfEmpty(0).Sum();
            return sum;
        }

        private int GetTotalSpentMoney(string userEmail)
        {
            var purchases = _purRep.GetAllByUserEmail(userEmail);
            var sum = purchases.Select(x => x.Cost).DefaultIfEmpty(0).Sum();
            return sum;
        }

    }
}
