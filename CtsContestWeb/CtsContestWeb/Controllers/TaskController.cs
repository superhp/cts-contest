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

        public IEnumerable<TaskDto> Get()
        {
            var tasks = TaskManager.GetAllTasks();

            return new List<TaskDto>
            {
                new TaskDto
                {
                    Id = 1,
                    Name = "1st task",
                    Value = 1,
                    Description = "hey ho lets go"
                },
                new TaskDto
                {
                    Id = 2,
                    Name = "2nd task",
                    Value = 2,
                    Description = "lets go hey ho"
                },
                new TaskDto
                {
                    Id = 3,
                    Name = "3rd task",
                    Value = 2,
                    Description = "tralialia"
                },
                new TaskDto
                {
                    Id = 4,
                    Name = "Task for 5 points",
                    Value = 5
                },
                new TaskDto
                {
                    Id = 5,
                    Name = "Another task for 5 points",
                    Value = 5
                },
                new TaskDto()
                {
                    Id = 6,
                    Name = "Task for 3 points",
                    Value = 3
                }
            };
        }

        [HttpGet("{id}")]
        public TaskDto Get(int id)
        {
            var task = TaskManager.GetTaskById(id);

            return new TaskDto
            {
                Id = 1,
                Name = "Task by ID",
                Value = 11
            };
        }

        [HttpPut("[action]")]
        public void Solve(string source)
        {
            throw new NotImplementedException();
        }
    }
}
