using System;
using Microsoft.AspNetCore.Mvc;
using CtsContestWeb.Communication;
using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        public ITask TaskManager { get; }

        public TaskController(ITask taskManager)
        {
            TaskManager = taskManager;
        }

        public async Task<IEnumerable<TaskDto>> Get()
        {
            var tasks = await TaskManager.GetAllTasks();
            return tasks;
        }

        [HttpGet("{id}")]
        public async Task<TaskDto> Get(int id)
        {
            var task = await TaskManager.GetTaskById(id);
            return task;
        }

        [HttpPut("[action]")]
        public void Solve(string source)
        {
            throw new NotImplementedException();
        }
    }
}
