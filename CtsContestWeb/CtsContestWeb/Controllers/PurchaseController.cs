using System;
using System.Threading.Tasks;
using CtsContestWeb.Communication;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using Microsoft.AspNetCore.Mvc;
using CtsContestWeb.Logic;
using System.Security.Claims;
using CtsContestWeb.Filters;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly IPrizeRepository _prizeRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseLogic _purchaseLogic;
        private readonly IBalanceLogic _balanceLogic;

        public PurchaseController(IPurchaseRepository purchaseRepository, IPrizeRepository prizeRepository, IPurchaseLogic purchaseLogic, IBalanceLogic balanceLogic)
        {
            _purchaseRepository = purchaseRepository;
            _prizeRepository = prizeRepository;
            _purchaseLogic = purchaseLogic;
            _balanceLogic = balanceLogic;
        }

        [HttpGet("[action]/{id}")]
        public PurchaseDto GetPrizeByPurchaseGuid(Guid id)
        {
            var purchase = _purchaseRepository.GetPurchaseByPurchaseGuid(id);

            var prizeDto = _prizeRepository.GetPrizeById(purchase.PrizeId);

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
        public PurchaseIdDto Purchase([FromBody] PurchaseRequestDto req)
        {
            var prizeId = req.PrizeId;
            var userEmail = User.FindFirst(ClaimTypes.Email).Value;
            var prize = _prizeRepository.GetPrizeById(prizeId);

            var canBuy = _purchaseLogic.CheckIfUserCanBuy(userEmail, prizeId);
            if (!canBuy)
            {
                throw new Exception();   //Don't know what kind of specific exception should be here
            }

            var id = _purchaseRepository.Create(userEmail, prizeId, prize.Price);

            return new PurchaseIdDto { Id = id };
        }
    }
}
