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
        private readonly IPrizeManager _prizeManager;

        public PrizeController(IPrizeManager prizeManager)
        {
            _prizeManager = prizeManager;
        }

        [HttpGet("")]
        public async Task<List<PrizeDto>> Get()
        {
           return await _prizeManager.GetAllPrizes();
        }

        [HttpGet("{id}")]
        public async Task<PrizeDto> Get(int id)
        {
            return await _prizeManager.GetPrizeById(id);
        }
    }
}
