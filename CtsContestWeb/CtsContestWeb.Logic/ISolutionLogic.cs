using System.Threading.Tasks;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Logic
{
    public interface ISolutionLogic
    {
        Task<CompileDto> CheckSolution(int taskId, string source, int language);
        Task SaveSolution(int taskId, string source, string userEmail, int language, bool isCorrect = true);
        void SaveDuelSolution(int taskId, string source, string userEmail, int language, bool resultCorrect);
    }
}
