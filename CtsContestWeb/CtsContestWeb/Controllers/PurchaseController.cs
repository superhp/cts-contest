using System;
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
    [Authorize]
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly IPrizeManager _prizeManager;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly PurchaseLogic _purchaseLogic;

        public PurchaseController(IPurchaseRepository purchaseRepository, IPrizeManager prizeManager, PurchaseLogic purchaseLogic)
        {
            _purchaseRepository = purchaseRepository;
            _prizeManager = prizeManager;
            _purchaseLogic = purchaseLogic;
        }

        public Guid Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("[action]")]
        public bool GiveAway([FromBody] PurchaseIdDto id)
        {
            var giveAwaySuccessful = _purchaseRepository.GiveAway(id.Id);
            return giveAwaySuccessful;
        }


        [HttpPost("[action]")]
        public async Task<PurchaseIdDto> Purchase([FromBody] PurchaseRequestDto req)
        {
            var prizeId = req.PrizeId;
            var userEmail = User.FindFirst(ClaimTypes.Email).Value;
            var prize = await _prizeManager.GetPrizeById(prizeId);

            var id = _purchaseRepository.Create(userEmail, prizeId, prize.Price);
            return new PurchaseIdDto { Id = id };
        }

        
    }
}
