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
            var task = await _taskManager.GetTaskById(taskId);
            var compileResult = await _compiler.Compile(task, source, language);

            if (compileResult.Compiled && compileResult.ResultCorrect)
            {
                SaveSolution(task, source, userEmail, language);
            }

            return compileResult;
        }

        public void SaveSolution(TaskDto task, string source, string userEmail, int language, bool isCorrect = true)
        {
            var solution = _solutionRepository.GetSolution(userEmail, task.Id);

            if (solution == null)
            {
                solution = new Solution
                {
                    UserEmail = userEmail,
                    Created = DateTime.Now,
                    Score = task.Value,
                    Source = source,
                    TaskId = task.Id,
                    Language = language,
                    IsCorrect = isCorrect
                };
            }
            else
            {
                solution.Language = language;
                solution.Source = source;
                solution.IsCorrect = isCorrect;
            }

            _solutionRepository.Upsert(solution);
        }
    }
}
