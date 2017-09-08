using System;
using Microsoft.AspNetCore.Mvc;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        public void Get()
        {
            throw new NotImplementedException();
        }
        
        public void Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("[action]")]
        public void Solve(string source)
        {
            throw new NotImplementedException();
        }
    }
}
