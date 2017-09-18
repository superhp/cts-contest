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

        [HttpGet("[action]")]
        public async Task<Response<List<PrizeDto>>> Get()
        {
            var prizes = await PrizeManager.GetAllPrizes();

            return new Response<List<PrizeDto>>
            {
                Data = prizes
            };
        }

        [HttpGet("[action]/{id}")]
        public async Task<Response<PrizeDto>> Get(int id)
        {
            var prize = await PrizeManager.GetPrizeById(id);

            return new Response<PrizeDto>
            {
                Data = prize
            };
        }

        [HttpPut("[action]")]
        public void Buy(int id)
        {
            throw new NotImplementedException();
        }
    }
}
