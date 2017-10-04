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
            var balance = await GetCurrentBalance(userEmail); 
            return balance >= price;
        }

        public async Task<int> GetCurrentBalance(string userEmail)
        {
            return await GetTotalEarnedMoney(userEmail) - await GetTotalSpentMoney(userEmail);
        }

        private async Task<int> GetTotalEarnedMoney(string userEmail)
        {
            var ids = _solRep.GetTaskIdsByUserEmail(userEmail);
            var tasks = await System.Threading.Tasks.Task.WhenAll(ids.Select(x => _taskManager.GetTaskById(x)));
            var sum = tasks.Select(x => x.Value).DefaultIfEmpty(0).Sum();
            return sum;
        }

        private async Task<int> GetTotalSpentMoney(string userEmail)
        {
            var purchases = await _purRep.GetAllByUserEmail(userEmail);
            var sum = purchases.Select(x => x.Cost).DefaultIfEmpty(0).Sum();
            return sum;
        }

    }
}
