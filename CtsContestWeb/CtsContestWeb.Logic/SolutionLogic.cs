using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CtsContestWeb.Communication;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using Task = System.Threading.Tasks.Task;

namespace CtsContestWeb.Logic
{
    public class SolutionLogic : ISolutionLogic
    {
        private readonly ITaskManager _taskManager;
        private readonly ICompiler _compiler;
        private readonly ISolutionRepository _solutionRepository;

        public SolutionLogic(ICompiler compiler, ISolutionRepository solutionRepository, ITaskManager taskManager)
        {
            _compiler = compiler;
            _solutionRepository = solutionRepository;
            _taskManager = taskManager;
        }

        public async Task<CompileDto> CheckSolution(int taskId, string source, int language, string userEmail)
        {
            var compileResult = await _compiler.Compile(taskId, source, language);

            if (compileResult.Compiled && compileResult.ResultCorrect)
            {
                await SaveSolution(taskId, source, userEmail);
            }

            return compileResult;
        }

        public async Task SaveSolution(int taskId, string source, string userEmail)
        {
            var task = await _taskManager.GetTaskById(taskId);

            var solution = new Solution
            {
                UserEmail = userEmail,
                Created = DateTime.Now,
                Score = task.Value,
                Source = source,
                TaskId = taskId
            };

            _solutionRepository.Create(solution);
        }
    }
}
