using CtsContestWeb.Communication;
using CtsContestWeb.Db.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CtsContestWeb.Logic
{
    public class BalanceLogic : IBalanceLogic
    {
        private readonly ISolutionRepository _solutionRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ITaskManager _taskManager;
        private readonly IPrizeManager _prizeManager;
        private readonly ICompetitionRepository _competitionRepository;

        public BalanceLogic(ISolutionRepository solutionRepository, ITaskManager taskManager, 
            IPurchaseRepository purchaseRepository, IPrizeManager prizeManager, ICompetitionRepository competitionRepository)
        {
            _solutionRepository = solutionRepository;
            _taskManager = taskManager;
            _purchaseRepository = purchaseRepository;
            _prizeManager = prizeManager;
            _competitionRepository = competitionRepository;
        }

        public async Task<bool> IsBalanceEnough(string userEmail, int prizeId)
        {
            var prize = await _prizeManager.GetPrizeById(prizeId);
            var price = prize.Price;
            var balance = GetCurrentBalance(userEmail); 
            return balance >= price;
        }

        public int GetTotalBalance(string userEmail)
        {
            return GetTotalEarnedMoney(userEmail) - GetTotalSpentMoney(userEmail);
        }

        public int GetCompetitionBalance(string userEmail)
        {
            var competitions = _competitionRepository.GetWonCompetitionsByEmail(userEmail);

            return competitions.Sum(c => c.Prize);
        }

        public int GetCurrentBalance(string userEmail)
        {
            return GetTodaysEarnedMoney(userEmail) - GetTodaysSpentMoney(userEmail);
        }

        private int GetTotalEarnedMoney(string userEmail)
        {
            var solutions = _solutionRepository.GetSolutionsByUserEmail(userEmail);
            var sum = solutions.Where(s => s.IsCorrect).Select(x => x.Score).DefaultIfEmpty(0).Sum();
            return sum;
        }
        private int GetTotalSpentMoney(string userEmail)
        {
            var purchases = _purchaseRepository.GetAllByUserEmail(userEmail);
            var sum = purchases.Select(x => x.Cost).DefaultIfEmpty(0).Sum();
            return sum;
        }

        private int GetTodaysEarnedMoney(string userEmail)
        {
            var solutions = _solutionRepository.GetSolutionsByUserEmail(userEmail);
            //var sum = solutions.Where(s => s.IsCorrect).Select(x => x.Score).DefaultIfEmpty(0).Sum();
            var sum = solutions.Where(s => s.IsCorrect && s.Created.Date == DateTime.Today).Select(x => x.Score).DefaultIfEmpty(0).Sum();
            return sum;
        }

        private int GetTodaysSpentMoney(string userEmail)
        {
            var purchases = _purchaseRepository.GetAllByUserEmail(userEmail);
            //var sum = purchases.Select(x => x.Cost).DefaultIfEmpty(0).Sum();
            var sum = purchases.Where(p => p.Created.Date == DateTime.Today).Select(x => x.Cost).DefaultIfEmpty(0).Sum();
            return sum;
        }   
    }
}
