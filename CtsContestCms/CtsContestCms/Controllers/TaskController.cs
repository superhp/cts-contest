using System;
using System.Collections.Generic;
using System.Linq;
using CtsContestCms.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using System.Text;
using CtsContestCms.Filters;

namespace CtsContestCms.Controllers
{
    [ContestIPHandler]
    public class TaskController : UmbracoApiController
    {
        // GET api/task/getInputs
        public List<object> GetInputs()
        {
            var taskDtos = new List<object>();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var tasks = umbracoHelper.Content(1590).Children;

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
        public IEnumerable<TaskDto> GetAll()
        {
            var taskDtos = GetAllTasks();

            return taskDtos.Where(t => !t.IsForCompetition);
        }

        // GET api/task/getAllCompetitionTasks
        public IEnumerable<TaskDto> GetAllCompetitionTasks()
        {
            var taskDtos = GetAllTasks();

            return taskDtos.Where(t => t.IsForCompetition);
        }

       // GET api/task/get/{id}
        public TaskDto Get(int id)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var task = umbracoHelper.Content(id);
            if (task.Id == 0 || !task.DocumentTypeAlias.Equals("taskNew"))
                throw new ArgumentException("No task found with given ID");

            return GetNewTaskDto(task);
        }

        private static List<TaskDto> GetAllTasks()
        {
            var taskDtos = new List<TaskDto>();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var tasks = umbracoHelper.Content(1590).Children;

            foreach (var task in tasks)
            {
                if (task.GetPropertyValue("enabled"))
                {
                    List<TestcaseDto> testCases = task.GetPropertyValue("testCases").ToObject<List<TestcaseDto>>();

                    IEnumerable<string> outputs = testCases.Select(x => x.Output);

                    if (outputs.Any())
                        taskDtos.Add(new TaskDto
                        {
                            Id = task.Id,
                            Name = task.Name,
                            Value = task.GetPropertyValue("value"),
                            IsEnabled = task.GetPropertyValue("enabled"),
                            IsForCompetition = task.GetPropertyValue("competition")
                        });
                }
            }

            return taskDtos;
        }

        private TaskDto GetNewTaskDto(dynamic task)
        {
            List<TestcaseDto> testCases = task.GetPropertyValue("testCases").ToObject<List<TestcaseDto>>();

            var inputs = testCases.Select(x => string.Join("\n", x.Input.Split('\n', '\r').Where(s => s.Length != 0).Select(s => s.Trim('\r', '\n', ' '))));
            var outputs = testCases.Select(x => x.Output);

            return new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = ConstructDescription(task, testCases),
                Value = task.GetPropertyValue("value"),
                Inputs = inputs,
                Outputs = outputs,
                InputType = task.GetPropertyValue("inputType")
            };
        }

        private string ConstructDescription(dynamic task, IEnumerable<TestcaseDto> testcases)
        {
            var sb = new StringBuilder();
            sb.Append(task.GetPropertyValue("description"));
            sb.Append("<p><strong>Input:</strong></p>");
            sb.Append(task.GetPropertyValue("inputFormat"));
            sb.Append("<p><strong>Output:</strong></p>");
            sb.Append(task.GetPropertyValue("outputFormat"));
            sb.Append("<p><strong>Example:</strong></p>");
            sb.Append(GenerateTestsTable(testcases));

            return sb.ToString();
        }

        private string GenerateTestsTable(IEnumerable<TestcaseDto> tests)
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