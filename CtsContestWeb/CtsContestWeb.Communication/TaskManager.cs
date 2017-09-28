using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CtsContestWeb.Communication
{
    public class TaskManager : ITaskManager
    {
        IConfiguration _iconfiguration;
        public TaskManager(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public async Task<List<TaskDto>> GetAllTasks()
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var pictureUrl = _iconfiguration["UmbracoPictureUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/getAll", Method.GET);

            TaskCompletionSource<List<TaskDto>> taskCompletion = new TaskCompletionSource<List<TaskDto>>();
            client.ExecuteAsync<List<TaskDto>>(request, response =>
            {
                taskCompletion.SetResult(response.Data);
            });
            var tasks = await taskCompletion.Task;
            foreach (var task in tasks)
            {
                task.Description = PrependRootUrlToImageLinks(task.Description, pictureUrl);
            }
            return tasks;
        }

        public async Task<TaskDto> GetTaskById(int id)
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
            task.Description = PrependRootUrlToImageLinks(task.Description, pictureUrl);
            return task;
        }
        private String PrependRootUrlToImageLinks(string description, string url)
        {
            var htmlPattern = @"(src="")(/media/(.+?)"")";
            var newDescription = Regex.Replace(description, htmlPattern, "$1" + url + "$2");
            return newDescription;
        }
    }
}
