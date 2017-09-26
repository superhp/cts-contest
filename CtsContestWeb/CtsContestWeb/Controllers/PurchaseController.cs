using System;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Communication;
using System.Threading.Tasks;

namespace CtsContestWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        public IPrize PrizeManager { get; }
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseController(IPurchaseRepository purchaseRepository, IPrize prizeManager)
        {
            _purchaseRepository = purchaseRepository;
            PrizeManager = prizeManager;
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

        //[HttpGet("{id}")]
        //public async Task<PurchaseIdDto> Purchase(int prizeId, string userEmail)
        [HttpPost("[action]")]
        public async Task<PurchaseIdDto> Purchase([FromBody] PurchaseRequestDto req)
        {
            //iskviest repositorija
            //tureti metoda, kuris sukuria ta irasa
            //grazint Guid'a pakurta i fronteanda

            var prizeId = req.PrizeId;
            var userEmail = req.UserEmail;

            var prize = await PrizeManager.GetPrizeById(prizeId);
            var purchase = new Db.Entities.Purchase
            {
                UserEmail = userEmail,
                PrizeId = prizeId,
                Created = DateTime.Now,
                PurchaseId = Guid.NewGuid(),
                Cost = prize.Price
            };
            //_purchaseRepository.Create(purchase);
            return new PurchaseIdDto { Id = purchase.PurchaseId };
        }
    }
}
