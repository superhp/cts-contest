using System;
using System.Collections.Generic;
using System.Linq;
using CtsContestCms.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace CtsContestCms.Controllers
{
    public class TaskController : UmbracoApiController
    {
        // GET api/task/getAll
        public List<TaskDto> GetAll()
        {
            var taskDtos = new List<TaskDto>();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var tasks = umbracoHelper.Content(1153).Children;

            foreach (var task in tasks)
            {
                taskDtos.Add(new TaskDto
                {
                    Id = task.Id,
                    Name = task.Name,
                    Value = task.GetPropertyValue("value"),
                });
            }

            return taskDtos;
        }

        // GET api/task/get/{id}
        public TaskDto Get(int id)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var task = umbracoHelper.Content(id);
            if (task.Id == 0 || !task.DocumentTypeAlias.Equals("task"))
                throw new ArgumentException("No task found with given ID");

            var taskDto = GetTaskDto(task);

            return taskDto;
        }

        private TaskDto GetTaskDto(dynamic task)
        {
            string[] inputsRaw = task.GetPropertyValue("input");
            var inputs = inputsRaw.Where(i => i != string.Empty);

            string[] outputsRaw = task.GetPropertyValue("output");
            var outputs = outputsRaw.Where(i => i != string.Empty);

            return new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.GetPropertyValue("description").ToString(),
                Value = task.GetPropertyValue("value"),
                Inputs = inputs,
                Outputs = outputs
            };
        }

    }
}