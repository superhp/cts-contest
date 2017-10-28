using System;
using System.Collections.Generic;
using System.Linq;
using CtsContestCms.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using System.Text;

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
                        InputType = task.GetPropertyValue("inputType"),
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
                    IsEnabled = task.GetPropertyValue("enabled")
                });
            }

            return taskDtos.Where(t => t.IsEnabled).ToList();
        }

        // GET api/task/get/{id}
        public TaskDto Get(int id)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var task = umbracoHelper.Content(id);
            if (task.Id == 0 || (!task.DocumentTypeAlias.Equals("task") && !task.DocumentTypeAlias.Equals("taskNew")))
                throw new ArgumentException("No task found with given ID");

            var taskDto = task.DocumentTypeAlias.Equals("task") ? GetTaskDto(task) : GetNewTaskDto(task);

            return taskDto;
        }

        private TaskDto GetNewTaskDto (dynamic task)
        {
            List<TestcaseDto> testCases = task.GetPropertyValue("testCases").ToObject<List<TestcaseDto>>();
            string testsTable = GenerateTestsTable(testCases);
            
            IEnumerable<string> inputs = testCases.Select(x => x.Input);
            IEnumerable<string> outputs = testCases.Select(x => x.Output);

            return new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.GetPropertyValue("description").ToString() + "<p><strong>Example:</strong></p>" + testsTable,
                Value = task.GetPropertyValue("value"),
                Inputs = inputs,
                Outputs = outputs,
                InputType = task.GetPropertyValue("inputType")
            };
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

        private string GenerateTestsTable (IEnumerable<TestcaseDto> tests)
        {
            var sb = new StringBuilder();
            sb.Append("<table>");
            sb.Append("<thead><tr><th>Input</th><th>Output</th></tr></thead>");
            sb.Append("<tbody>");
            var samples = tests.Where(t => t.IsSample);
            samples.ToList().ForEach(s => sb.Append("<tr><td>")
                                            .Append(s.Input.Replace("\n", "<br/>"))
                                            .Append("</td><td>")
                                            .Append(s.Output.Replace("\n", "<br/>"))
                                            .Append("</td></tr>"));
            sb.Append("</tbody></table>");
            return sb.ToString();
        }
    }
}