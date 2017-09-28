using System;
using Microsoft.AspNetCore.Mvc;
using CtsContestWeb.Communication;
using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class PrizeController : Controller
    {
        public IPrizeManager PrizeManager { get; }

        public PrizeController(IPrizeManager prizeManager)
        {
            PrizeManager = prizeManager;
        }

        [HttpGet("")]
        public async Task<List<PrizeDto>> Get()
        {
           return await PrizeManager.GetAllPrizes();
        }

        [HttpGet("{id}")]
        public async Task<PrizeDto> Get(int id)
        {
            return await PrizeManager.GetPrizeById(id);
        }

        [HttpPut("[action]")]
        public void Buy(int id)
        {
            throw new NotImplementedException();
        }
    }
}
