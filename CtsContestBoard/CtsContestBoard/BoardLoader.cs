using CtsContestBoard.Dto;
using DotNetify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public class Record
        {
            public string Name { get; set; }
            public int Score { get; set; }
        }

        public string LastUpdate => DateTime.Now.ToString();

        private List<Record> _board = new List<Record>();
        public List<Record> Board => _board;

        private List<PrizeDto> _prizes = new List<PrizeDto>();
        public List<PrizeDto> Prizes => _prizes;

        public bool ShowLeaderboard { get; set; }

        private int _a = 10;
        public int A => _a;

        private Timer _timer;
        private List<Solution> _solutions;
        private List<Purchase> _purchases;

        public BoardLoader(IPrizeManager prizeManager, ISolutionRepository solutionRepository, IPurchaseRepository purchaseRepository)
        {
            _prizeManager = prizeManager;
            _solutionRepository = solutionRepository;
            _purchaseRepository = purchaseRepository;

            _prizes = _prizeManager.GetAllPrizes().Result;
            _solutions = _solutionRepository.GetAll().ToList();
            _purchases = _purchaseRepository.GetAll().ToList();

            _timer = new Timer(state =>
            {
                _a += 1;
                InvertShowLeaderBoard();

                if (ShowLeaderboard)
                {
                    UpdateBoard();
                    Changed(nameof(Board));
                }
                else
                {
                    UpdatePrizes();
                    Changed(nameof(Prizes));
                }

                Changed(nameof(Board));
                Changed(nameof(A));

                Changed(nameof(ShowLeaderboard));
                Changed(nameof(LastUpdate));

                PushUpdates();
            }, null, 0, 5000);
        }

        private void UpdateBoard()
        {
            var lastId = _solutions.Max(s => s.SolutionId);
            var newSolutions = _solutionRepository.GetAll().Where(s => s.SolutionId > lastId);
            _solutions.AddRange(newSolutions);

            _board = _solutions.GroupBy(s => s.UserEmail).Select(ss => new Record
            {
                Name = ss.First().UserEmail,
                Score = ss.Sum(sc => sc.Score)
            }).ToList();
        }

        private void UpdatePrizes()
        {
            var lastDate = _purchases.Max(s => s.Created);
            var newPurchases = _purchaseRepository.GetAll().Where(s => s.Created > lastDate);
            _purchases.AddRange(newPurchases);

            //_prizes = 
        }

        private void InvertShowLeaderBoard()
        {
            ShowLeaderboard = !ShowLeaderboard;
        }

        public override void Dispose() => _timer.Dispose();
    }
}