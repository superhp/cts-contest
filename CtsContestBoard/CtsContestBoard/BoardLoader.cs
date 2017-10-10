using CtsContestBoard.Dto;
using DotNetify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CtsContestBoard.Communications;
using CtsContestBoard.Db.Entities;
using CtsContestBoard.Db.Repository;

namespace CtsContestBoard
{
    public class BoardLoader : BaseVM
    {
        private readonly IPrizeManager _prizeManager;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        public string LastUpdate => DateTime.Now.ToString();

        private List<ParticipantDto> _board = new List<ParticipantDto>();
        public List<ParticipantDto> Board => _board;

        private List<PrizeDto> _prizes = new List<PrizeDto>();
        public List<PrizeDto> Prizes => _prizes;

        public bool ShowLeaderboard { get; set; }

        private Timer _timer;
        private List<Solution> _solutions;
        private List<Purchase> _purchases;

        public BoardLoader(IPrizeManager prizeManager, ISolutionRepository solutionRepository, IPurchaseRepository purchaseRepository)
        {
            _prizeManager = prizeManager;
            _solutionRepository = solutionRepository;
            _purchaseRepository = purchaseRepository;

            _solutions = _solutionRepository.GetAll().ToList();

            _prizes = _prizeManager.GetAllPrizes().Result;
            _purchases = _purchaseRepository.GetAll().ToList();
            UpdatePrizes(_purchases);

            _timer = new Timer(state =>
            {
                InvertShowLeaderBoard();

                if (ShowLeaderboard)
                {
                    UpdateBoard();
                    Changed(nameof(Board));
                }
                else
                {
                    var newPurchases =  GetNewPurchases();
                    UpdatePrizes(newPurchases);
                    Changed(nameof(Prizes));
                }

                Changed(nameof(ShowLeaderboard));
                Changed(nameof(LastUpdate));

                PushUpdates();
            }, null, 0, 5000);
        }

        private void UpdateBoard()
        {
            int lastId = 0;
            if (_solutions.Count > 0)
                lastId = _solutions.Max(s => s.SolutionId);

            var spentPoints = _purchases.GroupBy(p => p.UserEmail)
                .Select(gp => new {UserEmail = gp.First().UserEmail, Points = gp.Sum(p => p.Cost)}).ToList();
            var newSolutions = _solutionRepository.GetAll().Where(s => s.SolutionId > lastId).ToList();
            _solutions.AddRange(newSolutions);

            var groupedSolutions = _solutions.GroupBy(s => s.UserEmail).ToList();

            _board = groupedSolutions.Select(ss => new ParticipantDto
            {
                Name = ss.First().User.FullName,
                Picture = ss.First().User.Picture,
                TotalEarnedPoints = ss.Sum(sc => sc.Score),
                Balance = ss.Sum(sc => sc.Score) - spentPoints.Where(p => p.UserEmail.Equals(ss.First().User.Email)).Sum(p => p.Points),
                TodayEarnedPoints = ss.Where(s => s.Created.Date == DateTime.Today).Sum(p => p.Score)
            }).ToList();
        }

        private List<Purchase> GetNewPurchases()
        {
            var lastDate = _purchases.Max(s => s.Created);
            var newPurchases = _purchaseRepository.GetAll().Where(s => s.Created > lastDate).ToList();
            _purchases.AddRange(newPurchases);

            return newPurchases;
        }

        private void UpdatePrizes(List<Purchase> newPurchases)
        {
            _prizes = _prizes.Select(p => new PrizeDto
            {
                Id = p.Id,
                Name = p.Name,
                Picture = p.Picture,
                Price = p.Price,
                Quantity = p.Quantity - newPurchases.Count(np => np.PrizeId == p.Id)
            }).ToList();
        }

        private void InvertShowLeaderBoard()
        {
            ShowLeaderboard = !ShowLeaderboard;
        }

        public override void Dispose() => _timer.Dispose();
    }
}