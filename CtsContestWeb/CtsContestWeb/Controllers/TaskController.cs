using System;
using Microsoft.AspNetCore.Mvc;
using CtsContestWeb.Communication;
using CtsContestWeb.Dto;
using System.Collections.Generic;

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
        public Response<List<TaskDto>> Get()
        {
            var tasks = TaskManager.GetAllTasks();

            return new Response<List<TaskDto>>
            {
                Data = tasks
            };
        }
        
        [HttpGet("[action]/{id}")]
        public Response<TaskDto> Get(int id)
        {
            var task = TaskManager.GetTaskById(id);

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
