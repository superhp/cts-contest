using CtsContestWeb.Communication;
using CtsContestWeb.Db;
using CtsContestWeb.Db.DataAccess;
using CtsContestWeb.Db.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CtsContestWeb.Logic
{
    public class BalanceLogic
    {
        private int _amount;
        private readonly ISolutionRepository _solRep;
        private readonly IPurchaseRepository _purRep;
        public ITask TaskManager { get; }
        public IPrize PrizeManager { get; }

        public BalanceLogic(ISolutionRepository solRep, ITask taskManager, IPurchaseRepository purRep, IPrize prizeManager)
        {
            _amount = 0;
            _solRep = solRep;
            TaskManager = taskManager;
            _purRep = purRep;
            PrizeManager = prizeManager;
        }

        public int Amount
        {
            get { return _amount; }
        }

        public async Task<bool> IsBalanceEnough(string userEmail, int prizeId)
        {
            var prize = await PrizeManager.GetPrizeById(prizeId);
            var price = prize.Price;
            var balance = (await GetTotalEarnedMoney(userEmail)) - GetTotalSpentMoney(userEmail); 
            return balance >= price;
        }

        private async Task<int> GetTotalEarnedMoney(string userEmail)
        {
            var ids = _solRep.GetTaskIdsByUserEmail(userEmail);
            var tasks = await System.Threading.Tasks.Task.WhenAll(ids.Select(x => TaskManager.GetTaskById(x)));
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
