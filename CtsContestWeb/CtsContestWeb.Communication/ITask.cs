using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Communication
{
    public interface ITask
    {
        Task<List<TaskDto>> GetAllTasks();
        Task<TaskDto> GetTaskById(int id);
    }
}