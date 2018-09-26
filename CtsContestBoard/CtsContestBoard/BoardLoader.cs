using CtsContestBoard.Dto;
using DotNetify;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using CtsContestBoard.Communications;
using CtsContestBoard.Db;
using CtsContestBoard.Db.Entities;
using CtsContestBoard.Db.Repository;

namespace CtsContestBoard
{
    public class BoardLoader : BaseVM
    {
        private readonly IPrizeManager _prizeManager;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _dbContext;

        public string LastUpdate => DateTime.Now.ToString();
        private readonly List<PrizeDto> _prizes;

        private List<ParticipantDto> _leaderBoard = new List<ParticipantDto>();
        public List<ParticipantDto> LeaderBoard => _leaderBoard;

        private List<PrizeDto> _prizesForPoints = new List<PrizeDto>();
        public List<PrizeDto> PrizesForPoints => _prizesForPoints;

        private PrizeAndApplicantDto _todaysPrize = new PrizeAndApplicantDto();
        public PrizeAndApplicantDto TodaysPrize => _todaysPrize;

        private PrizeAndApplicantDto _weeksPrize = new PrizeAndApplicantDto();
        public PrizeAndApplicantDto WeeksPrize => _weeksPrize;

        public BoardEnum Board { get; set; }

        private readonly Timer _timer;
        private readonly List<Solution> _solutions;
        private readonly List<Purchase> _purchases;
        private List<User> _users;

        public enum BoardEnum
        {
            LeaderBoard,
            Prizes,
            TodayPrizes,
            WeekPrizes,
            Information,
            JobPosters,
            Slogan
        }

        private readonly List<BoardEnum> _ignoredBoards = new List<BoardEnum> { BoardEnum.TodayPrizes };

        public BoardLoader(IPrizeManager prizeManager, ISolutionRepository solutionRepository, IPurchaseRepository purchaseRepository, IUserRepository userRepository, ApplicationDbContext dbContext)
        {
            _prizeManager = prizeManager;
            _solutionRepository = solutionRepository;
            _purchaseRepository = purchaseRepository;
            _userRepository = userRepository;
            _dbContext = dbContext;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            lock (dbContext)
            {
                _solutions = _solutionRepository.GetAll().Where(s => s.IsCorrect).ToList();

                _prizes = _prizeManager.GetAllPrizes().Result;
                _purchases = _purchaseRepository.GetAll().ToList();
                _users = _userRepository.GetAll().ToList();

                UpdateLeaderBoard();
            }

            _timer = new Timer(state =>
            {
                try
                {
                    lock (dbContext)
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
                                    Changed(nameof(TodaysPrize));
                                    break;
                                }
                            case BoardEnum.WeekPrizes:
                                {
                                    GetNewPurchases();
                                    UpdateWeekPrizes();
                                    Changed(nameof(WeeksPrize));
                                    break;
                                }
                        }

                        Changed(nameof(Board));
                        Changed(nameof(LastUpdate));

                        PushUpdates();
                    }
                }
                catch (Exception ex){};
            }, null, 0, 15000);
        }

        private void UpdateTodayPrizes()
        {
            var weekday = DateTime.Today.ToString("dddd");
            _todaysPrize = UpdatePrizesByCategory($"{weekday} prize");
        }

        private void UpdateWeekPrizes()
        {
            _weeksPrize = UpdatePrizesByCategory("Week prize");
        }

        private PrizeAndApplicantDto UpdatePrizesByCategory(string category)
        {
            List<ParticipantDto> participants;

            if (category.Equals("Week prize"))
            {
                participants = _leaderBoard.OrderByDescending(p => p.TotalBalance).ThenByDescending(p => p.LastSolved)
                    .ToList();
            }
            else
            {
                var dayPrizes = _purchases.Where(p => p.PrizeId == 1548 || p.PrizeId == 1550 || p.PrizeId == 1303).Select(p => p.UserEmail).ToList();
                participants = _leaderBoard.Where(l => dayPrizes.All(dp => dp != l.Email)).OrderByDescending(p => p.TodaysBalance).ThenByDescending(p => p.LastSolved)
                    .ToList();
            }

            var applicantsForPrize = 3;
            var participantsNeeded = applicantsForPrize + _prizes.Count(p => p.Category.Equals(category));
            participants = participants.Take(participantsNeeded).ToList();
            if (participantsNeeded > participants.Count)
                while (participantsNeeded > participants.Count)
                    participants.Add(new ParticipantDto());

            var i = 0;
            var prize = _prizes.FirstOrDefault(p => p.Category.Equals(category));

            if (prize == null)
                return new PrizeAndApplicantDto();

            return new PrizeAndApplicantDto
            {
                Id = prize.Id,
                Name = prize.Name,
                Picture = prize.Picture,
                Price = prize.Price,
                Quantity = prize.Quantity - _purchases.Count(np => np.PrizeId == prize.Id),
                Category = prize.Category,
                Applicants = participants.Skip(i++).Take(applicantsForPrize)
            };
        }

        private void UpdateLeaderBoard()
        {
            UpdateSolutions();
            UpdateUsers();

            _leaderBoard = _users.Select(u => new ParticipantDto
            {
                Name = u.FullName,
                Email = u.Email,
                Picture = u.Picture,
                LastSolved = _solutions.Where(s => s.UserEmail.Equals(u.Email) && s.IsCorrect)
                        .DefaultIfEmpty(new Solution()).Max(s => s.Created),
                TotalBalance = _solutions.Where(s => s.UserEmail.Equals(u.Email) && s.IsCorrect).Sum(s => s.Score) -
                                   _purchases.Where(s => s.UserEmail.Equals(u.Email)).Sum(s => s.Cost),
                TodaysBalance =
                        _solutions.Where(
                                s => s.UserEmail.Equals(u.Email) && s.Created.Date == DateTime.Today && s.IsCorrect)
                            .Sum(s => s.Score) -
                        _purchases.Where(s => s.UserEmail.Equals(u.Email) && s.Created.Date == DateTime.Today)
                            .Sum(s => s.Cost)
            }).OrderByDescending(l => l.TodaysBalance).ThenByDescending(l => l.TotalBalance)
                .ThenByDescending(l => l.LastSolved).ToList();
        }

        private void UpdateUsers()
        {
            _users = _userRepository.GetAll().ToList();
        }

        private void UpdateSolutions()
        {
            int lastId = 0;
            if (_solutions.Count > 0)
                lastId = _solutions.Max(s => s.SolutionId);

            var newSolutions = _solutionRepository.GetAll().Where(s => s.SolutionId > lastId && s.IsCorrect).ToList();
            _solutions.AddRange(newSolutions);
        }

        private void GetNewPurchases()
        {
            DateTime lastDate = DateTime.MinValue;

            if (_purchases.Count > 0)
                lastDate = _purchases.Max(s => s.Created);
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
            /*if (Board < BoardEnum.Information)
                Board++;
            else
                Board = BoardEnum.LeaderBoard;*/
            do
            {
                Board = Increment(Board);
            } while (_ignoredBoards.Contains(Board));
        }

        private BoardEnum Increment(BoardEnum value)
        {
            if (value < BoardEnum.JobPosters) return value + 1;
            return BoardEnum.LeaderBoard;
        }

        public override void Dispose() => _timer.Dispose();
    }
}