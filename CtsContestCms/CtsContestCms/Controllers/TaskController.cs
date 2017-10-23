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
        // GET api/task/getInputs
        public List<object> GetInputs()
        {
            var taskDtos = new List<object>();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var tasks = umbracoHelper.Content(1153).Children;

            foreach (var task in tasks)
            {
                string[] inputsRaw = task.GetPropertyValue("input");
                var inputs = inputsRaw.Where(i => i != string.Empty).Select(str => str.Replace("\\n", "\n")).ToList();

                string[] outputsRaw = task.GetPropertyValue("output");
                var outputs = outputsRaw.Where(i => i != string.Empty).Select(str => str.Replace("\\n", "\n")).ToList();

                var n = inputs.Count;
                for (var i = 0; i < n; i++)
                {
                    taskDtos.Add(new
                    {
                        Id = task.Id,
                        Input = inputs[i],
                        Output = outputs[i],
                        InputType = task.GetPropertyValue("inputType")
                    });
                }

            }

            return taskDtos;
        }

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
            var inputs = inputsRaw.Where(i => i != string.Empty).Select(str => str.Replace("\\n", "\n"));

            string[] outputsRaw = task.GetPropertyValue("output");
            var outputs = outputsRaw.Where(i => i != string.Empty).Select(str => str.Replace("\\n", "\n"));

            return new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.GetPropertyValue("description").ToString(),
                Value = task.GetPropertyValue("value"),
                Inputs = inputs,
                Outputs = outputs,
                InputType = task.GetPropertyValue("inputType")
            };
        }
    }
}