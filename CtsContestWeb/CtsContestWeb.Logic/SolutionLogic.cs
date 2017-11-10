using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using CtsContestWeb.Communication;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task = System.Threading.Tasks.Task;

namespace CtsContestWeb.Logic
{
    public class SolutionLogic : ISolutionLogic
    {
        private readonly ITaskManager _taskManager;
        private readonly IConfiguration _iconfiguration;
        private readonly IIndex<string, ICompiler> _compilers;
        private readonly ISolutionRepository _solutionRepository;

        public SolutionLogic(ISolutionRepository solutionRepository, ITaskManager taskManager, IConfiguration iconfiguration, IIndex<string, ICompiler> compilers)
        {
            _solutionRepository = solutionRepository;
            _taskManager = taskManager;
            _iconfiguration = iconfiguration;
            _compilers = compilers;
        }

        public async Task<CompileDto> CheckSolution(int taskId, string source, int language, string userEmail)
        {
            var task = await _taskManager.GetTaskById(taskId);
            CompileDto compileResult;

            try
            {
                var compilerName = _iconfiguration["Compiler"];
                var compiler = _compilers[compilerName];
                compileResult = await compiler.Compile(task, source, language);
            }
            catch (Exception e)
            {
                compileResult = await CompileWithBackupCompiler(task, source, language);
            }

            if (compileResult.Compiled && compileResult.ResultCorrect)
            {
                SaveSolution(task, source, userEmail, language);
            }

            return compileResult;
        }

        private async Task<CompileDto> CompileWithBackupCompiler(TaskDto task, string source, int language)
        {
            var compilerName = _iconfiguration["BackupCompiler"];
            var compiler = _compilers[compilerName];
            return await compiler.Compile(task, source, language);
        }

        public void SaveSolution(TaskDto task, string source, string userEmail, int language, bool isCorrect = true)
        {
            var solution = _solutionRepository.GetSolution(userEmail, task.Id);

            if (solution != null && solution.IsCorrect)
            {
                throw new Exception($"You can't save solved solution. User: {userEmail}. TaskId: {task.Id}.");
            }

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
                solution.Score = task.Value;
            }

            _solutionRepository.Upsert(solution);
        }
    }
}
