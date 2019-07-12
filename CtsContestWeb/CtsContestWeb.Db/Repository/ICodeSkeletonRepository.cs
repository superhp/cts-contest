using System.Threading.Tasks;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Db.Repository
{
    public interface ICodeSkeletonRepository
    {
        Task<CodeSkeletonDto> GetCodeSkeleton(LanguageDto languages, string userEmail, int taskId, string language);
    }
}
