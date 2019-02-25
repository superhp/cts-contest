using CtsContestWeb.Communication;
using CtsContestWeb.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class PuzzleController : Controller
    {
        private readonly IPuzzleManager _puzzleManager;

        public PuzzleController(IPuzzleManager puzzleManager)
        {
            _puzzleManager = puzzleManager;
        }

        public async Task<IEnumerable<PuzzleDto>> Get()
        {
            return await _puzzleManager.GetPuzzles();
        }
    }
}
