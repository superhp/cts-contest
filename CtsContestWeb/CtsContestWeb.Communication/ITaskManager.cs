using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Communication
{
    public interface ITaskManager
    {
        Task<List<TaskDto>> GetAllTasks(string userEmail = null);
        Task<TaskDto> GetTaskById(int id, string userEmail = null);
        Task<CodeSkeletonDto> GetCodeSkeleton(string language);
    }
}