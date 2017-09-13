using CtsContestWeb.Dto;
using System.Collections.Generic;

namespace CtsContestWeb.Communication
{
    public interface ITask
    {
        List<TaskDto> GetAllTasks();
        TaskDto GetTaskById(int id);
    }
}