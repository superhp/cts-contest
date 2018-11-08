using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CtsContestWeb.Db.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace CtsContestWeb.Communication
{
    public class TaskManager : ITaskManager
    {
        private readonly IConfiguration _iconfiguration;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IDuelRepository _duelRepository;
        private readonly IMemoryCache _cache;

        private const string RegularTasksCacheKey = "regularTasks";
        private const string DuelTasksCacheKey = "duelTasks";

        public TaskManager(IConfiguration iconfiguration, ISolutionRepository solutionRepository, IDuelRepository duelRepository, IMemoryCache cache)
        {
            _iconfiguration = iconfiguration;
            _solutionRepository = solutionRepository;
            _duelRepository = duelRepository;
            _cache = cache;
        }

        public async Task<TaskDto> DownloadTaskByIdAsync(int id, string userEmail = null)
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var pictureUrl = _iconfiguration["UmbracoPictureUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/get/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());

            var response = await client.ExecuteTaskAsync<TaskDto>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ArgumentException("No task with given ID");
            var task = response.Data;

            if (userEmail != null)
            {
                var solvedTasks = _solutionRepository.GetSolvedTasksIdsByUserEmail(userEmail);
                if (solvedTasks.Any(t => t == task.Id))
                    task.IsSolved = true;
            }
            task.Description = PrependRootUrlToImageLinks(task.Description, pictureUrl);
            UpdateTaskValue(task);
            return task;
        }

        public async Task<TaskDto> GetTaskForDuelAsync(IEnumerable<string> usersEmail)
        {
            var duelTaskIds = (await GetTasks(DuelTasksCacheKey)).Select(t => t.Id).ToList();
            var competitions = usersEmail.SelectMany(email => _duelRepository.GetDuelsByEmail(email));

            var usedTaskIds = competitions.Select(c => c.Task.Id).Distinct();
            var availableTaskIds = duelTaskIds.Where(id => !usedTaskIds.Contains(id)).ToList();

            if (availableTaskIds.Count == 0) return null;

            var taskId = new Random().Next(availableTaskIds.Count);
            var task = await GetTaskAsync(DuelTasksCacheKey, availableTaskIds[taskId]);

            return task;
        }

        public async Task<List<TaskDto>> GetAllTasks(string userEmail = null)
        {
            var tasks = await GetTasks(RegularTasksCacheKey);
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

        private void CacheTasks(List<TaskDto> tasks, string name)
        {
            MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
            cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
            cacheExpirationOptions.Priority = CacheItemPriority.Normal;
            _cache.Set<List<TaskDto>>(name, tasks, cacheExpirationOptions);
        }

        private async Task<List<TaskDto>> GetTasks(string cacheKey)
        {
            List<TaskDto> tasks;
            if (!_cache.TryGetValue(cacheKey, out tasks))
            {
                var allTasks = await DownloadAllTasksAsync();
                var duelTasks = allTasks.Where(x => x.IsForDuel).ToList();
                CacheTasks(duelTasks, DuelTasksCacheKey);
                var regularTasks = allTasks.Where(x => !x.IsForDuel).ToList();
                CacheTasks(regularTasks, RegularTasksCacheKey);
                return cacheKey == DuelTasksCacheKey ? duelTasks : regularTasks;
            }
            return tasks;
        }

        private async Task<TaskDto> GetTaskAsync(string cacheKey, int id)
        {
            List<TaskDto> tasks;
            if (!_cache.TryGetValue(cacheKey, out tasks))
            {
                return await DownloadTaskByIdAsync(id);
            }
            return tasks.Find(x => x.Id == id);
        }

        private async Task<List<TaskDto>> DownloadAllTasksAsync()
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/getAll", Method.GET);

            TaskCompletionSource<List<TaskDto>> taskCompletion = new TaskCompletionSource<List<TaskDto>>();
            client.ExecuteAsync<List<TaskDto>>(request, response => { taskCompletion.SetResult(response.Data); });

            var tasks = await taskCompletion.Task;

            tasks.ForEach(UpdateTaskValue);
            return tasks;
        }

        private string PrependRootUrlToImageLinks(string description, string url)
        {
            var htmlPattern = @"(src="")(/media/(.+?)"")";
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
