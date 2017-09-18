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
        public IPrize PrizeManager { get; }

        public PrizeController(IPrize prizeManager)
        {
            PrizeManager = prizeManager;
        }

        [HttpGet("")]
        public async Task<List<PrizeDto>> Get()
        {
            var prizes = await PrizeManager.GetAllPrizes();

            return prizes;
        }

        [HttpGet("{id}")]
        public async Task<PrizeDto> Get(int id)
        {
            var prize = await PrizeManager.GetPrizeById(id);

            return prize;
        }

        [HttpPut("[action]")]
        public void Buy(int id)
        {
            throw new NotImplementedException();
        }
    }
}
