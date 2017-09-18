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

        [HttpGet("[action]")]
        public async Task<Response<List<TaskDto>>> Get()
        {
            var tasks = await TaskManager.GetAllTasks();

            return new Response<List<TaskDto>>
            {
                Data = tasks
            };
        }
        
        [HttpGet("[action]/{id}")]
        public async Task<Response<TaskDto>> Get(int id)
        {
            var task = await TaskManager.GetTaskById(id);

            return new Response<TaskDto>
            {
                Data = task
            };
        }

        [HttpPut("[action]")]
        public void Solve(string source)
        {
            throw new NotImplementedException();
        }
    }
}
