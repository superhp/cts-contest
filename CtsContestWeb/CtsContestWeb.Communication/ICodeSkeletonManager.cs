using System.Threading.Tasks;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Communication
{
    public interface ICodeSkeletonManager
    {
        Task<CodeSkeletonDto> GetCodeSkeleton(string userEmail, int taskId, string language);
        string GenerateCodeSkeletonForTask(TaskDto task, GenericCodeSkeletonDto genericSkeleton);
    }
}
