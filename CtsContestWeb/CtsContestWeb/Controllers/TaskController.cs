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
        public ITaskManager TaskManager { get; }
        public ICompiler Compiler { get; }

        public TaskController(ITaskManager taskManager, ICompiler compiler)
        {
            TaskManager = taskManager;
            Compiler = compiler;
        }

        public async Task<IEnumerable<TaskDto>> Get()
        {
            return await TaskManager.GetAllTasks();
        }

        [HttpGet("{id}")]
        public async Task<TaskDto> Get(int id)
        {
            return await TaskManager.GetTaskById(id);
        }

        [HttpPut("[action]")]
        public async Task<CompileDto> Solve(int taskId, string source, int language)
        {
            return await Compiler.Compile(taskId, source, language);
        }

        [HttpGet("[action]")]
        public async Task<LanguageDto> GetLanguages()
        {
            return await Compiler.GetLanguages();
        }
    }
}
