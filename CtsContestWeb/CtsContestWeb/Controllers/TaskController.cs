using Microsoft.AspNetCore.Mvc;
using CtsContestWeb.Communication;
using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CtsContestWeb.Logic;
using Microsoft.AspNetCore.Authorization;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskManager _taskManager;
        private readonly ICompiler _compiler;
        private readonly ISolutionLogic _solutionLogic;

        public TaskController(ITaskManager taskManager, ICompiler compiler, ISolutionLogic solutionLogic)
        {
            _taskManager = taskManager;
            _compiler = compiler;
            _solutionLogic = solutionLogic;
        }

        public async Task<IEnumerable<TaskDto>> Get()
        {
            string userEmail = null;
            if (User.Identity.IsAuthenticated)
                userEmail = User.FindFirst(ClaimTypes.Email).Value;

            return await _taskManager.GetAllTasks(userEmail);
        }

        [HttpGet("{id}")]
        public async Task<TaskDto> Get(int id)
        {
            string userEmail = null;
            if (User.Identity.IsAuthenticated) 
                userEmail = User.FindFirst(ClaimTypes.Email).Value;

            return await _taskManager.GetTaskById(id, userEmail);
        }

        [Authorize]
        [HttpPut("[action]")]
        public async Task<CompileDto> Solve(int taskId, string source, int language)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email).Value;
            return await _solutionLogic.CheckSolution(taskId, source, language, userEmail);
        }

        [HttpGet("[action]")]
        public async Task<LanguageDto> GetLanguages()
        {
            return await _compiler.GetLanguages();
        }
    }
}
