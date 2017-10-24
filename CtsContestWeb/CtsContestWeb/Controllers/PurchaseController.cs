using System;
using System.Threading.Tasks;
using CtsContestWeb.Communication;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using Microsoft.AspNetCore.Mvc;
using CtsContestWeb.Logic;
using System.Security.Claims;
using CtsContestWeb.Filters;
using Microsoft.AspNetCore.Authorization;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly IPrizeManager _prizeManager;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseLogic _purchaseLogic;
        private readonly IBalanceLogic _balanceLogic;

        public PurchaseController(IPurchaseRepository purchaseRepository, IPrizeManager prizeManager, IPurchaseLogic purchaseLogic, IBalanceLogic balanceLogic)
        {
            _purchaseRepository = purchaseRepository;
            _prizeManager = prizeManager;
            _purchaseLogic = purchaseLogic;
            _balanceLogic = balanceLogic;
        }

        [HttpGet("[action]/{id}")]
        public async Task<PurchaseDto> GetPrizeByPurchaseGuid(Guid id)
        {
            var purchase = _purchaseRepository.GetPurchaseByPurchaseGuid(id);

            var prizeDto = await _prizeManager.GetPrizeById(purchase.PrizeId);

            purchase.Price = prizeDto.Price;
            purchase.Name = prizeDto.Name;
            purchase.Picture = prizeDto.Picture;

            purchase.BalanceLeft = _balanceLogic.GetCurrentBalance(purchase.UserEmail);

            return purchase;
        }

        [HttpPost("[action]")]
        public GiveAwayResponse GiveAway(Guid id)
        {
            var giveAwaySuccessful = _purchaseRepository.GiveAway(id);

            return new GiveAwayResponse
            {
                IsGivenAway = giveAwaySuccessful
            };
        }

        [LoggedIn]
        [HttpPost("[action]")]
        public async Task<PurchaseIdDto> Purchase([FromBody] PurchaseRequestDto req)
        {
            var prizeId = req.PrizeId;
            var userEmail = User.FindFirst(ClaimTypes.Email).Value;
            var prize = await _prizeManager.GetPrizeById(prizeId);

            var canBuy = await _purchaseLogic.CheckIfUserCanBuy(userEmail, prizeId);
            if (!canBuy)
                throw new Exception();   //Don't know what kind of specific exception should be here

            var id = _purchaseRepository.Create(userEmail, prizeId, prize.Price);

            return new PurchaseIdDto { Id = id };
        }
    }
}
