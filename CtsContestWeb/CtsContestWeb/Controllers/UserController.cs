using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using CtsContestWeb.Duel;
using CtsContestWeb.Logic;
using Microsoft.AspNetCore.Mvc;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IBalanceLogic _balanceLogic;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDuelRepository _duelRepository;

        public UserController(IBalanceLogic balanceLogic, IPurchaseRepository purchaseRepository,
            IUserRepository userRepository, IDuelRepository duelRepository)
        {
            _balanceLogic = balanceLogic;
            _purchaseRepository = purchaseRepository;
            _userRepository = userRepository;
            _duelRepository = duelRepository;
        }

        [HttpGet("")]
        public UserInfoDto GetUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                _userRepository.InsertIfNotExists(User);
                return new UserInfoDto
                {
                    Email = User.FindFirst(ClaimTypes.Email).Value,
                    Name = User.FindFirst(ClaimTypes.GivenName).Value,
                    TodaysBalance = _balanceLogic.GetCurrentBalance(User.FindFirst(ClaimTypes.Email).Value),
                    TotalBalance = _balanceLogic.GetTotalBalance(User.FindFirst(ClaimTypes.Email).Value),
                    DuelBalance = _balanceLogic.GetDuelBalance(User.FindFirst(ClaimTypes.Email).Value),
                    IsLoggedIn = true
                };
            }

            return new UserInfoDto
            {
                IsLoggedIn = false
            };
        }

        [HttpGet("canbuy")]
        public bool CanBuyPrizes()
        {
            if (User.Identity.IsAuthenticated)
            {
                var email = User.FindFirst(ClaimTypes.Email).Value;
                return !UserHandler.IsPlayerInDuel(email);
            }

            return false;
        }

        [HttpGet("purchases")]
        public List<PurchaseDto> GetUserPurchases()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email).Value;
                var purchases = _purchaseRepository.GetAllByUserEmail(userEmail);

                return purchases.ToList().Select(p => new PurchaseDto
                {
                    PrizeId = p.PrizeId,
                    Price = p.Cost,
                    IsGivenAway = p.GivenPurchase != null,
                    PurchaseId = p.PurchaseId,
                    UserEmail = userEmail
                }).ToList();
            }

            return new List<PurchaseDto>();
        }

        [HttpGet("duel-statistics")]
        public UserDuelStatisticsDto GetUserDuelStatistics()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email).Value;
                return new UserDuelStatisticsDto
                {
                    Email = userEmail,
                    TotalWins = _duelRepository.GetWonDuelsByEmail(userEmail).Count(),
                    TotalLooses = _duelRepository.GetLostDuelsByEmail(userEmail).Count()
                };
            }

            return new UserDuelStatisticsDto();
        }

        [HttpGet("users")]
        public List<UserInfoDto> GetUsers()
        {
            return _userRepository.GetAllUsers().ToList();
        }
    }
}