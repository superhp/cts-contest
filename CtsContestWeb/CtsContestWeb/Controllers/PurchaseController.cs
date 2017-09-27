using System;
using System.Threading.Tasks;
using CtsContestWeb.Communication;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPrize _prizeManager;

        public PurchaseController(IPurchaseRepository purchaseRepository, IPrize prizeManager)
        {
            _purchaseRepository = purchaseRepository;
            _prizeManager = prizeManager;
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

        public bool Purchase()
        {
            //iskviest repositorija
            //tureti metoda, kuris sukuria ta irasa
            //grazint Guid'a pakurta i fronteanda
            return false;
        }
    }
}
