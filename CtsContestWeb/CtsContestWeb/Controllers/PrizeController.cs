using System;
using Microsoft.AspNetCore.Mvc;
using CtsContestWeb.Communication;
using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using CtsContestWeb.Db.Repository;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class PrizeController : Controller
    {
        private readonly IPrizeRepository _prizeRepository;

        public PrizeController(IPrizeRepository prizeRepository)
        {
            _prizeRepository = prizeRepository;
        }

        [HttpGet("[action]")]
        public List<PrizeDto> GetWinnables()
        {
            return _prizeRepository.GetAllWinnablePrizes();
        }

        [HttpGet("")]
        public List<PrizeDto> Get()
        {
           return _prizeRepository.GetAllPrizesForPoints();
        }

        [HttpGet("{id}")]
        public PrizeDto Get(int id)
        {
            return _prizeRepository.GetPrizeById(id);
        }
    }
}
