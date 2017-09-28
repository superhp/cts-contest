using CtsContestWeb.Communication;
using CtsContestWeb.Db;
using CtsContestWeb.Db.DataAccess;
using CtsContestWeb.Db.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CtsContestWeb.Logic
{
    public class BalanceLogic : IBalanceLogic
    {
        private int _amount;
        private readonly ISolutionRepository _solRep;
        private readonly IPurchaseRepository _purRep;
        private readonly ITaskManager _taskManager;
        private readonly IPrizeManager _prizeManager;

        public BalanceLogic(ISolutionRepository solRep, ITaskManager taskManager, IPurchaseRepository purRep, IPrizeManager prizeManager)
        {
            _amount = 0;
            _solRep = solRep;
            _taskManager = taskManager;
            _purRep = purRep;
        }

        public int Amount
        {
            get { return _amount; }
            _prizeManager = prizeManager;
        }

        public async Task<bool> IsBalanceEnough(string userEmail, int prizeId)
        {
            var prize = await _prizeManager.GetPrizeById(prizeId);
            var price = prize.Price;
            var balance = (await GetTotalEarnedMoney(userEmail)) - GetTotalSpentMoney(userEmail); 
            return balance >= price;
        }

        private async Task<int> GetTotalEarnedMoney(string userEmail)
        {
            var ids = _solRep.GetTaskIdsByUserEmail(userEmail);
            var tasks = await System.Threading.Tasks.Task.WhenAll(ids.Select(x => _taskManager.GetTaskById(x)));
            var sum = tasks.Select(x => x.Value).DefaultIfEmpty(0).Sum();
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
