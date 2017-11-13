﻿using CtsContestWeb.Dto;
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
        private readonly IMemoryCache _cache;

        public TaskManager(IConfiguration iconfiguration, ISolutionRepository solutionRepository, IMemoryCache cache)
        {
            _iconfiguration = iconfiguration;
            _solutionRepository = solutionRepository;
            _cache = cache;
        }

        public async Task<List<TaskDto>> GetAllTasks(string userEmail = null)
        {
            List<TaskDto> content;
            if (!_cache.TryGetValue<List<TaskDto>>("tasks", out content))
            {
                content =  await CacheTasks();
            }
            return content;
        }

        public async Task<List<TaskDto>> CacheTasks(string userEmail = null)
        {
            var content = await GetAllTasksFromApi(userEmail);

            MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
            cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
            cacheExpirationOptions.Priority = CacheItemPriority.Normal;
            _cache.Set<List<TaskDto>>("tasks", content, cacheExpirationOptions);

            return content;
        }

        private async Task<List<TaskDto>> GetAllTasksFromApi(string userEmail)
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/getAll", Method.GET);

            TaskCompletionSource<List<TaskDto>> taskCompletion = new TaskCompletionSource<List<TaskDto>>();
            client.ExecuteAsync<List<TaskDto>>(request, response => { taskCompletion.SetResult(response.Data); });

            var tasks = await taskCompletion.Task;
            var solvedTasks = _solutionRepository.GetSolvedTasksIdsByUserEmail(userEmail).ToList();

            foreach (var task in tasks)
            {
                if (userEmail != null)
                {
                    if (solvedTasks.Any(t => t == task.Id))
                        task.IsSolved = true;
                }
            }
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

            TaskCompletionSource<TaskDto> taskCompletion = new TaskCompletionSource<TaskDto>();
            client.ExecuteAsync<TaskDto>(request, response =>
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ArgumentException("No task with given ID");
                taskCompletion.SetResult(response.Data);
            });

            var task = await taskCompletion.Task;
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

        private string PrependRootUrlToImageLinks(string description, string url)
        {
            var htmlPattern = @"(src="")(/media/(.+?)"")";
            var newDescription = Regex.Replace(description, htmlPattern, "$1" + url + "$2");

            return newDescription;
        }

        private void UpdateTaskValue(TaskDto task)
        {
            double d = 8.2d * task.Value * task.Value - 20 * task.Value + 23;
            task.Value = (int)Math.Ceiling(d / 5) * 5;
        }
    }
}
