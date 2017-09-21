using System.Threading.Tasks;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Communication
{
    public interface ICompiler
    {
        Task<LanguageDto> GetLanguages();
        Task<CompileDto> Compile(int taskId, string source, int language);
    }
}