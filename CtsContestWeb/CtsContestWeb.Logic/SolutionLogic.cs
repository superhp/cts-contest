using System;
using System.Linq;
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
        private readonly ITaskRepository _taskRepository;
        private readonly IConfiguration _configuration;
        private readonly IIndex<string, ICompiler> _compilers;
        private readonly IDuelRepository _duelRepository;
        private readonly ISolutionRepository _solutionRepository;

        public SolutionLogic(ISolutionRepository solutionRepository, ITaskRepository taskRepository, IConfiguration configuration, IIndex<string, ICompiler> compilers, IDuelRepository duelRepository)
        {
            _solutionRepository = solutionRepository;
            _taskRepository = taskRepository;
            _configuration = configuration;
            _compilers = compilers;
            _duelRepository = duelRepository;
        }

        public async Task<CompileDto> CheckSolution(int taskId, string source, int language)
        {
            if (source.ToLower().Contains("fullcontact.com"))
                return new CompileDto
                {
                    Compiled = false,
                    FailedInput = 1,
                    Message = "Hello, World! :)",
                    TotalInputs = 1
                };

            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task.Outputs.Count == 0)

            {
                throw new Exception($"TaskId={taskId} doesn't have any inputs/outputs.");
            }

            return await Compile(task, source, language);
        }

        private async Task<CompileDto> Compile(TaskDto task, string source, int language, int compilerId = 0)
        {
            CompileDto compileResult;
            var compilerOptions = _configuration.GetSection("Compilers").GetChildren().Select(c => c.Value).ToList();

            try
            {
                if (compilerId > 2)
                    return new CompileDto();

                var compiler = _compilers[compilerOptions[compilerId]];
                compileResult = await compiler.Compile(task, source, language);
            }
            catch (Exception e)
            {
                return await Compile(task, source, language, compilerId + 1);
            }

            return compileResult;
        }

        public async Task SaveSolution(int taskId, string source, string userEmail, int language, bool isCorrect = true)
        {
            var task = await _taskRepository.GetCachedTaskByIdAsync(taskId);
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

        public void SaveDuelSolution(int competitionId, string source, string userEmail, int language, bool resultCorrect)
        {
            var solution = _duelRepository.GetSolution(competitionId, userEmail);

            if (solution != null && solution.IsCorrect)
            {
                throw new Exception($"You can't save solved solution. User: {userEmail}. CompetitionId: {competitionId}.");
            }

            if (solution == null)
            {
                solution = new DuelSolution
                {
                    UserEmail = userEmail,
                    DuelId = competitionId,
                    IsCorrect = resultCorrect,
                    Language = language,
                    Source = source
                };
            }
            else
            {
                solution.IsCorrect = resultCorrect;
                solution.Source = source;
                solution.Language = language;
            }

            _duelRepository.UpsertSolution(solution);
        }
    }
}
