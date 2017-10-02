using System;
using System.Threading.Tasks;
using CtsContestWeb.Communication;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Communication;
using System.Threading.Tasks;
using System.Collections.Generic;
using CtsContestWeb.Logic;
using System.Security.Claims;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly IPrizeManager _prizeManager;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseLogic _purchaseLogic;

        public PurchaseController(IPurchaseRepository purchaseRepository, IPrizeManager prizeManager, IPurchaseLogic purchaseLogic)
        {
            _purchaseRepository = purchaseRepository;
            _prizeManager = prizeManager;
            _purchaseLogic = purchaseLogic;
        }

        [HttpGet("[action]/{id}")]
        public async Task<PurchaseDto> GetPrizeByPurchaseGuid(Guid id)
        {
            var purchase = _purchaseRepository.GetPurchaseByPurchaseGuid(id);

            var prizeDto = await _prizeManager.GetPrizeById(purchase.PrizeId);

            purchase.Price = prizeDto.Price;
            purchase.Name = prizeDto.Name;
            purchase.Picture = prizeDto.Picture;

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


        [HttpPost("[action]")]
        public async Task<PurchaseIdDto> Purchase([FromBody] PurchaseRequestDto req)
        {
            var prizeId = req.PrizeId;
            var userEmail = User.FindFirst(ClaimTypes.Email).Value;
            var prize = await _prizeManager.GetPrizeById(prizeId);

            if (!_purchaseLogic.CheckIfUserCanBuy(userEmail, prizeId)) throw new Exception();   //Don't know what kind of specific exception should be here
            
            var id = _purchaseRepository.Create(userEmail, prizeId, prize.Price);
            return new PurchaseIdDto { Id = id };
        }

        
    }
}
