using CtsContestBoard.Dto;
using DotNetify;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private List<PrizeDto> _prizes = new List<PrizeDto>();

        private List<ParticipantDto> _leaderBoard = new List<ParticipantDto>();
        public List<ParticipantDto> LeaderBoard => _leaderBoard;

        private List<PrizeDto> _prizesForPoints = new List<PrizeDto>();
        public List<PrizeDto> PrizesForPoints => _prizesForPoints;

        private List<PrizeAndApplicantDto> _todayPrizes = new List<PrizeAndApplicantDto>(); // prize and applicant dto
        public List<PrizeAndApplicantDto> TodayPrizes => _todayPrizes;

        private List<PrizeAndApplicantDto> _weekPrizes = new List<PrizeAndApplicantDto>();
        public List<PrizeAndApplicantDto> WeekPrizes => _weekPrizes;

        public BoardEnum Board { get; set; }

        private Timer _timer;
        private List<Solution> _solutions;
        private List<Purchase> _purchases;

        public enum BoardEnum
        {
            LeaderBoard,
            Prizes,
            TodayPrizes,
            WeekPrizes
        }

        public BoardLoader(IPrizeManager prizeManager, ISolutionRepository solutionRepository, IPurchaseRepository purchaseRepository)
        {
            _prizeManager = prizeManager;
            _solutionRepository = solutionRepository;
            _purchaseRepository = purchaseRepository;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            _solutions = _solutionRepository.GetAll().ToList();

            _prizes = _prizeManager.GetAllPrizes().Result;
            _purchases = _purchaseRepository.GetAll().ToList();
            UpdatePrizes();

            _timer = new Timer(state =>
            {
                SwitchBoard();

                switch (Board)
                {
                    case BoardEnum.LeaderBoard:
                        UpdateLeaderBoard();
                        Changed(nameof(LeaderBoard));
                        break;
                    case BoardEnum.Prizes:
                        {
                            GetNewPurchases();
                            UpdatePrizes();
                            Changed(nameof(PrizesForPoints));
                            break;
                        }
                    case BoardEnum.TodayPrizes:
                        {
                            GetNewPurchases();
                            UpdateTodayPrizes();
                            Changed(nameof(TodayPrizes));
                            break;
                        }
                    case BoardEnum.WeekPrizes:
                        {
                            GetNewPurchases();
                            UpdateWeekPrizes();
                            Changed(nameof(WeekPrizes));
                            break;
                        }
                }

                Changed(nameof(Board));
                Changed(nameof(LastUpdate));

                PushUpdates();
            }, null, 0, 5000);
        }

        private void UpdateTodayPrizes()
        {
            var weekday = DateTime.Today.ToString("dddd");
            _todayPrizes = UpdatePrizesByCategory($"{weekday} prize");
        }

        private void UpdateWeekPrizes()
        {
            _weekPrizes = UpdatePrizesByCategory("Week prize");
        }

        private List<PrizeAndApplicantDto> UpdatePrizesByCategory(string category)
        {
            List<ParticipantDto> participants;

            if (category.Equals("Week prize"))
                participants = _leaderBoard.OrderBy(p => p.Balance).ToList();
            else
                participants = _leaderBoard.OrderBy(p => p.TodayEarnedPoints).ToList();

            var prizesCount = _prizes.Count(p => p.Category.Equals(category));
            participants = participants.Take(prizesCount).ToList();
            if (prizesCount > participants.Count)
                while (prizesCount > participants.Count)
                    participants.Add(new ParticipantDto());

            var i = 0;
            return _prizes.Where(p => p.Category.Equals(category)).Select(p => new PrizeAndApplicantDto
            {
                Id = p.Id,
                Name = p.Name,
                Picture = p.Picture,
                Price = p.Price,
                Quantity = p.Quantity - _purchases.Count(np => np.PrizeId == p.Id),
                Category = p.Category,
                Applicant = participants[i++]
            }).ToList();
        }

        private void UpdateLeaderBoard()
        {
            int lastId = 0;
            if (_solutions.Count > 0)
                lastId = _solutions.Max(s => s.SolutionId);

            var spentPoints = _purchases.GroupBy(p => p.UserEmail)
                .Select(gp => new { UserEmail = gp.First().UserEmail, Points = gp.Sum(p => p.Cost) }).ToList();
            var newSolutions = _solutionRepository.GetAll().Where(s => s.SolutionId > lastId).ToList();
            _solutions.AddRange(newSolutions);

            var groupedSolutions = _solutions.GroupBy(s => s.UserEmail).ToList();

            _leaderBoard = groupedSolutions.Select(ss => new ParticipantDto
            {
                Name = ss.First().User.FullName,
                Picture = ss.First().User.Picture,
                TotalEarnedPoints = ss.Sum(sc => sc.Score),
                Balance = ss.Sum(sc => sc.Score) - spentPoints.Where(p => p.UserEmail.Equals(ss.First().User.Email)).Sum(p => p.Points),
                TodayEarnedPoints = ss.Where(s => s.Created.Date == DateTime.Today).Sum(p => p.Score)
            }).ToList();
        }

        private void GetNewPurchases()
        {
            var lastDate = _purchases.Max(s => s.Created);
            var newPurchases = _purchaseRepository.GetAll().Where(s => s.Created > lastDate).ToList();
            _purchases.AddRange(newPurchases);
        }

        private void UpdatePrizes()
        {
            _prizesForPoints = _prizes.Where(p => p.Category.Equals("Prize for points")).Select(p => new PrizeDto
            {
                Id = p.Id,
                Name = p.Name,
                Picture = p.Picture,
                Price = p.Price,
                Quantity = p.Quantity - _purchases.Count(np => np.PrizeId == p.Id),
                Category = p.Category
            }).ToList();
        }

        private void SwitchBoard()
        {
            if (Board < BoardEnum.WeekPrizes)
                Board++;
            else
                Board = BoardEnum.LeaderBoard;
        }

        public override void Dispose() => _timer.Dispose();
    }


}