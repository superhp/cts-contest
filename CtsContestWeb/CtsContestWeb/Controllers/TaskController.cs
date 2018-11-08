using System;
using Microsoft.AspNetCore.Mvc;
using CtsContestWeb.Communication;
using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CtsContestWeb.Filters;
using CtsContestWeb.Logic;
using Microsoft.Extensions.Configuration;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskManager _taskManager;
        private readonly ICompiler _compiler;
        private readonly ISolutionLogic _solutionLogic;
        private readonly ICodeSkeletonManager _codeSkeletonManager;
        private readonly IConfiguration _configuration;

        public TaskController(ITaskManager taskManager, ICompiler compiler, ISolutionLogic solutionLogic, ICodeSkeletonManager codeSkeletonManager, IConfiguration iconfiguration)
        {
            _taskManager = taskManager;
            _compiler = compiler;
            _solutionLogic = solutionLogic;
            _codeSkeletonManager = codeSkeletonManager;
            _configuration = iconfiguration;
        }

        public async Task<IEnumerable<TaskDisplayDto>> Get()
        {
            string userEmail = null;
            if (User.Identity.IsAuthenticated)
                userEmail = User.FindFirst(ClaimTypes.Email).Value;

            var tasks = await _taskManager.GetAllTasks(userEmail);
            return tasks.Select(t => new TaskDisplayDto
            {
                Id = t.Id,
                Name = t.Name,
                Value = t.Value,
                IsSolved = t.IsSolved
            });
        }

        [HttpGet("{id}")]
        public async Task<TaskDisplayDto> Get(int id)
        {
            string userEmail = null;
            if (User.Identity.IsAuthenticated) 
                userEmail = User.FindFirst(ClaimTypes.Email).Value;

            var task = await _taskManager.DownloadTaskByIdAsync(id, userEmail);

            return new TaskDisplayDto
            {
                Id = task.Id,
                Name = task.Name,
                Value = task.Value,
                Description = task.Description,
                IsSolved = task.IsSolved
            };
        }

        [LoggedIn]
        [HttpPut("[action]")]
        public async Task<CompileDto> Solve(int taskId, string source, int language)
        {
            if (IsCognizantChallengeOver())
            {
                throw new Exception("Challenge is over!");
            }  
            var userEmail = User.FindFirst(ClaimTypes.Email).Value;
            var compileResult = await _solutionLogic.CheckSolution(taskId, source, language);

            if (compileResult.Compiled && compileResult.ResultCorrect)
            {
                await _solutionLogic.SaveSolution(taskId, source, userEmail, language);
            }

            return compileResult;
        }

        [LoggedIn]
        [HttpPut("[action]")]
        public async Task SaveCode(int taskId, string source, int language)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email).Value;

            await _solutionLogic.SaveSolution(taskId, source, userEmail, language, false);
        }

        [HttpGet("[action]")]
        public LanguageDto GetLanguages()
        {
            return _compiler.GetLanguages();
        }

        [HttpGet("[action]/{language}/{taskId}")]
        public async Task<CodeSkeletonDto> GetCodeSkeleton(string language, int taskId)
        {
            string userEmail;
            if (User.FindFirst(ClaimTypes.Email) != null)
                userEmail = User.FindFirst(ClaimTypes.Email).Value;
            else
                userEmail = null;
            return await _codeSkeletonManager.GetCodeSkeleton(userEmail, taskId, language);
        }

        private bool IsCognizantChallengeOver()
        {
            var now = DateTime.UtcNow;
            var endTime = DateTime.Parse(_configuration.GetValue<string>("EndTimeUTC"));
            return now > endTime;
        }
    }
}
