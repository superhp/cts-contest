using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Communication
{
    public interface ITaskManager
    {
        Task<List<TaskDto>> GetAllTasks();
        Task<TaskDto> GetTaskById(int id);
    }
}