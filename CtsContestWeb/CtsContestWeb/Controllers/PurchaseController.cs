using System;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseController(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
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
    }
}
