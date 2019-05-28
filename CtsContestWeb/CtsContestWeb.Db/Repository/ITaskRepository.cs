using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Db.Repository
{
    public interface ITaskRepository
    {
        Task<List<TaskDto>> GetAllTasks(string userEmail = null);
        Task<TaskDto> GetCachedTaskByIdAsync(int id, string userEmail = null);
        Task<int?> GetTaskIdForDuelAsync(IEnumerable<string> usersEmail);
        Task<bool> HasPlayerAnyDuelTasksLeft(string userEmail);
        void RemoveTasksCache();
    }
}
