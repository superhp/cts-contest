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
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IMemoryCache _cache;

        public TaskManager(IConfiguration iconfiguration, ISolutionRepository solutionRepository, ICompetitionRepository competitionRepository, IMemoryCache cache)
        {
            _iconfiguration = iconfiguration;
            _solutionRepository = solutionRepository;
            _competitionRepository = competitionRepository;
            _cache = cache;
        }

        public async Task<List<TaskDto>> GetAllTasks(string userEmail = null)
        {
            List<TaskDto> tasks;
            if (!_cache.TryGetValue<List<TaskDto>>("tasks", out tasks))
            {
                tasks = await GetAllTasksFromApi();
                CacheTasks(tasks);
            }

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

        private async Task<List<TaskDto>> GetAllTasksForCompetition()
        {
            var competitionTasks = "competitionTasks";
            List<TaskDto> tasks;

            if (!_cache.TryGetValue<List<TaskDto>>(competitionTasks, out tasks))
            {
                tasks = await GetAllTasksForCompetitionFromApi();
                CacheTasks(tasks, competitionTasks);
            }

            return tasks;
        }

        private async Task<List<TaskDto>> GetAllTasksForCompetitionFromApi()
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/getAllCompetitionTasks", Method.GET);

            TaskCompletionSource<List<TaskDto>> taskCompletion = new TaskCompletionSource<List<TaskDto>>();
            client.ExecuteAsync<List<TaskDto>>(request, response => { taskCompletion.SetResult(response.Data); });

            var tasks = await taskCompletion.Task;

            tasks.ForEach(UpdateTaskValue);
            return tasks;
        }

        public void CacheTasks(List<TaskDto> tasks, string name = "tasks")
        {
            MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
            cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
            cacheExpirationOptions.Priority = CacheItemPriority.Normal;
            _cache.Set<List<TaskDto>>(name, tasks, cacheExpirationOptions);
        }

        private async Task<List<TaskDto>> GetAllTasksFromApi()
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

        public async Task<TaskDto> GetTaskById(int id, string userEmail = null)
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

        public async Task<TaskDto> GetTaskForCompetition(IEnumerable<string> usersEmail)
        {
            var tasks = (await GetAllTasksForCompetition()).Select(t => t.Id).ToList();

            var competitions = new List<CompetitionDto>();
            foreach (var email in usersEmail)
            {
                competitions.AddRange(_competitionRepository.GetCompetitionsByEmail(email));
            }

            var usedTaskIds = competitions.Select(c => c.Task.Id).Distinct();
            var availableTaskIds = tasks.Where(t => !usedTaskIds.Contains(t)).ToList();

            if (availableTaskIds.Count == 0)
                return null;

            var rnd = new Random();
            var taskNr = rnd.Next(availableTaskIds.Count);

            var task = await GetTaskById(availableTaskIds[taskNr]);

            return task;
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
