using CtsContestWeb.Dto;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CtsContestWeb.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Task = CtsContestWeb.Db.Entities.Task;

namespace CtsContestWeb.Db.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ISolutionRepository _solutionRepository;
        private readonly IDuelRepository _duelRepository;
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _dbContext;
        private const string TaskCacheKey = "tasks";

        public TaskRepository(ISolutionRepository solutionRepository,
            IDuelRepository duelRepository, IMemoryCache cache, ApplicationDbContext dbContext)
        {
            _solutionRepository = solutionRepository;
            _duelRepository = duelRepository;
            _cache = cache;
            _dbContext = dbContext;
        }

        public async Task<TaskDto> GetTaskById(int id)
        {
            var task = await _dbContext.Tasks.Include(x => x.TestCases).SingleAsync(x => x.Id == id);

            var dtoTask = GetNewTaskDto(task);

            UpdateTaskValue(dtoTask);
            return dtoTask;
        }

        public async Task<TaskDto> GetCachedTaskByIdAsync(int id, string userEmail = null)
        {
            var cacheKey = "TaskNr" + id;
            TaskDto task;
            if (_cache.TryGetValue(cacheKey, out TaskDto cachedTask))
            {
                task = cachedTask;
            }
            else
            {
                task = await GetTaskById(id);
                _cache.Set(cacheKey, task);
            }

            if (userEmail != null)
            {
                var solvedTasks = _solutionRepository.GetSolvedTasksIdsByUserEmail(userEmail);
                if (solvedTasks.Any(t => t == task.Id))
                    task.IsSolved = true;
            }

            return task;
        }

        public async Task<int?> GetTaskIdForDuelAsync(IEnumerable<string> usersEmail)
        {
            var duelTaskIds = (await GetTasks()).Where(task => task.IsForDuel).Select(t => t.Id).ToList();
            var competitions = usersEmail.SelectMany(email => _duelRepository.GetDuelsByEmail(email));

            var usedTaskIds = competitions.Select(c => c.Task.Id).Distinct();
            var availableTaskIds = duelTaskIds.Where(id => !usedTaskIds.Contains(id)).ToList();

            if (availableTaskIds.Count == 0) return null;

            return availableTaskIds[new Random().Next(availableTaskIds.Count)];
        }

        public async Task<bool> HasPlayerAnyDuelTasksLeft(string userEmail)
        {
            var duelTaskIds = (await GetTasks()).Where(task => task.IsForDuel).Select(t => t.Id).ToList();
            var seenTaskIds = _duelRepository.GetDuelsByEmail(userEmail).Select(t => t.Task.Id);
            return duelTaskIds.Except(seenTaskIds).Any();
        }

        public void RemoveTasksCache()
        {
            _cache.Remove(TaskCacheKey);
        }

        public async Task<List<TaskDto>> GetAllTasks(string userEmail = null)
        {
            var tasks = (await GetTasks())/*.Where(task => !task.IsForDuel)*/.ToList();
            var solvedTasks = _solutionRepository.GetSolvedTasksIdsByUserEmail(userEmail).ToList();

            foreach (var task in tasks)
            {
                task.IsSolved = false;
                if (userEmail != null)
                {
                    if (solvedTasks.Any(t => t == task.Id))
                        task.IsSolved = true;
                }
            }

            return tasks;
        }

        private async Task<List<TaskDto>> GetTasks()
        {
            if (_cache.TryGetValue(TaskCacheKey, out List<TaskDto> tasks))
            {
                return tasks;
            }

            var allTasks = await GetTasksFromDb();
            CacheTasks(allTasks, TaskCacheKey);
            return allTasks;
        }

        private async Task<List<TaskDto>> GetTasksFromDb()
        {
            var tasks = await _dbContext.Tasks.Where(x => x.Enabled).ToListAsync();
            var dtoTasks = new List<TaskDto>();

            foreach (var task in tasks)
            {
                var dtoTask = GetNewTaskDto(task, false);

                UpdateTaskValue(dtoTask);
                dtoTasks.Add(dtoTask);
            }

            return dtoTasks;
        }

        private void CacheTasks(List<TaskDto> tasks, string name)
        {
            MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
            cacheExpirationOptions.SlidingExpiration = TimeSpan.FromMinutes(30);
            cacheExpirationOptions.Priority = CacheItemPriority.Normal;
            _cache.Set<List<TaskDto>>(name, tasks, cacheExpirationOptions);
        }

        private string PrependRootUrlToImageLinks(string description, string url)
        {
            const string htmlPattern = @"(src="")(/media/(.+?)"")";
            var newDescription = Regex.Replace(description, htmlPattern, "$1" + url + "$2");

            return newDescription;
        }

        private void UpdateTaskValue(TaskDto task)
        {
            if (task.Value == 0)
                task.Value = 10;
            else
            {
                var d = 8.2d * task.Value * task.Value - 20 * task.Value + 23;
                task.Value = (int)Math.Ceiling(d / 5) * 5;
            }
        }

        private TaskDto GetNewTaskDto(Task task, bool individualTask = true)
        {
            List<TaskTestCase> testCases = task.TestCases;

            List<string> inputs = new List<string>();
            List<string> outputs = new List<string>();

            if (individualTask)
            {
                inputs = testCases.Select(x => string.Join("\n", x.Input.Split('\n', '\r').Where(s => s.Length != 0).Select(s => s.Trim('\r', '\n', ' ')))).ToList();
                outputs = testCases.Select(x => x.Output).ToList();
            }

            return new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = individualTask ? ConstructDescription(task, testCases) : null,
                Value = task.Value,
                Inputs = inputs,
                Outputs = outputs,
                InputType = task.InputType
            };
        }

        private string ConstructDescription(Task task, List<TaskTestCase> testcases)
        {
            var sb = new StringBuilder();
            sb.Append(task.Description);
            sb.Append("<p><strong>Input:</strong></p>");
            sb.Append(task.InputType);
            sb.Append("<p><strong>Output:</strong></p>");
            sb.Append(task.OutputType);
            sb.Append("<p><strong>Example:</strong></p>");
            sb.Append(GenerateTestsTable(testcases));

            return sb.ToString();
        }

        private string GenerateTestsTable(List<TaskTestCase> tests)
        {
            var sb = new StringBuilder();
            sb.Append("<table>");
            sb.Append("<thead><tr><th>Input</th><th>Output</th></tr></thead>");
            sb.Append("<tbody>");
            var samples = tests.Where(t => t.IsSample);
            samples.ToList().ForEach(s => sb.Append("<tr><td>")
                                            .Append(s.Input.Replace("\n", "<br/>"))
                                            .Append("</td><td>")
                                            .Append(s.Output.Replace("\n", "<br/>"))
                                            .Append("</td></tr>"));
            sb.Append("</tbody></table>");
            return sb.ToString();
        }
    }
}