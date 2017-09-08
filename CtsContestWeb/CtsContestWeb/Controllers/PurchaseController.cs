using System;
using Microsoft.AspNetCore.Mvc;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        public Guid Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("[action]")]
        public bool GiveAway(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
