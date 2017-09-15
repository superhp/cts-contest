using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;

namespace CtsContestWeb.Communication
{
    public class Task : ITask
    {
        IConfiguration _iconfiguration;
        public Task(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public List<TaskDto> GetAllTasks()
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/getAll", Method.GET);

            var response = client.Execute<List<TaskDto>>(request);

            return response.Data;
        }
        
        public TaskDto GetTaskById(int id)
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/get/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString()); 

            var response = client.Execute<TaskDto>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ArgumentException("No task with given ID");

            return response.Data;
        }
    }
}
