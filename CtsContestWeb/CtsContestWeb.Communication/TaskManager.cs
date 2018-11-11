using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CtsContestWeb.Db.Repository;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace CtsContestWeb.Communication
{
    public class TaskManager : ITaskManager
    {
        private readonly IConfiguration _iconfiguration;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IDuelRepository _duelRepository;
        private readonly IMemoryCache _cache;

        private const string TaskCacheKey = "tasks";

        public TaskManager(IConfiguration iconfiguration, ISolutionRepository solutionRepository,
            IDuelRepository duelRepository, IMemoryCache cache)
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

        public async Task<TaskDto> GetCachedTaskByIdAsync(int id)
        {
            var cachedTask = (await GetTasks()).FirstOrDefault(task => task.Id == id);
            return cachedTask ?? await DownloadTaskByIdAsync(id);
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

        public async Task<List<TaskDto>> GetAllTasks(string userEmail = null)
        {
            var tasks = (await GetTasks()).Where(task => !task.IsForDuel).ToList();
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

            var allTasks = await DownloadAllTasksAsync();
            CacheTasks(allTasks, TaskCacheKey);
            return allTasks;
        }

        private async Task<List<TaskDto>> DownloadAllTasksAsync()
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var pictureUrl = _iconfiguration["UmbracoPictureUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/getAll", Method.GET);

            TaskCompletionSource<List<TaskDto>> taskCompletion = new TaskCompletionSource<List<TaskDto>>();
            client.ExecuteAsync<List<TaskDto>>(request, response => { taskCompletion.SetResult(response.Data); });

            var tasks = await taskCompletion.Task;

            foreach (var task in tasks)
            {
                task.Description = PrependRootUrlToImageLinks(task.Description, pictureUrl);
                UpdateTaskValue(task);
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
                task.Value = (int) Math.Ceiling(d / 5) * 5;
            }
        }
    }
}