using CtsContestWeb.Dto;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public TaskDto GetTaskById(int id)
        {
            /*  var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
              var pictureUrl = _iconfiguration["UmbracoPictureUrl"];
              var client = new RestClient(umbracoApiUrl);

              var request = new RestRequest("task/get/{id}", Method.GET);
              request.AddUrlSegment("id", id.ToString());

              var response = await client.ExecuteTaskAsync<TaskDto>(request);

              if (response.StatusCode != System.Net.HttpStatusCode.OK)
                  throw new ArgumentException("No task with given ID");
           //   var task = response.Data;

              task.Description = PrependRootUrlToImageLinks(task.Description, pictureUrl);
              */
            var task = _dbContext.Tasks.Include(x => x.TestCases).Single(x => x.Id == id);

            var dtoTask = new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                InputType = task.InputType,
                Inputs = task.TestCases == null ? new List<string>() : task.TestCases.Select((x => x.Input)).ToList(),
                Outputs = task.TestCases == null ? new List<string>() : task.TestCases.Select((x => x.Output)).ToList(),
                Value = task.Value
            };

            UpdateTaskValue(dtoTask);
            return dtoTask;
        }

        public async Task<TaskDto> GetCachedTaskByIdAsync(int id, string userEmail = null)
        {
            var cachedTask = (await GetTasks()).FirstOrDefault(t => t.Id == id);
            var task = cachedTask ?? GetTaskById(id);

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
            //    var tasks = (await GetTasks())/*.Where(task => !task.IsForDuel)*/.ToList();
            var tasks = GetTasksFromDb();
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

            var allTasks = GetTasksFromDb();
            CacheTasks(allTasks, TaskCacheKey);
            return allTasks;
        }

        private List<TaskDto> GetTasksFromDb()
        {
            /*
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var pictureUrl = _iconfiguration["UmbracoPictureUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/getAll", Method.GET);

            TaskCompletionSource<List<TaskDto>> taskCompletion = new TaskCompletionSource<List<TaskDto>>();
            client.ExecuteAsync<List<TaskDto>>(request, response => { taskCompletion.SetResult(response.Data); });

            var tasks = await taskCompletion.Task;
            */
            var tasks = _dbContext.Tasks.ToList();
            var dtoTasks = new List<TaskDto>();

            foreach (var task in tasks)
            {
                var dtoTask = new TaskDto
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    InputType = task.InputType,
                    Inputs = task.TestCases == null ? new List<string>() : task.TestCases.Select(x => x.Input).ToList(),
                    Outputs = task.TestCases == null ? new List<string>() : task.TestCases.Select((x => x.Output)).ToList(),
                    Value = task.Value
                };

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
    }
}