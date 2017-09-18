using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Communication
{
    public class Task : ITask
    {
        IConfiguration _iconfiguration;
        public Task(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public async Task<List<TaskDto>> GetAllTasks()
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/getAll", Method.GET);

            TaskCompletionSource<List<TaskDto>> taskCompletion = new TaskCompletionSource<List<TaskDto>>();
            client.ExecuteAsync<List<TaskDto>>(request, response =>
            {
                taskCompletion.SetResult(response.Data);
            });

            return await taskCompletion.Task;
        }

        public async Task<TaskDto> GetTaskById(int id)
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
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

            return await taskCompletion.Task;
        }
    }
}
