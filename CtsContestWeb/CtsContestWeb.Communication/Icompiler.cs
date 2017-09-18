using System.Threading.Tasks;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Communication
{
    public interface ICompiler
    {
        Task<LanguageDto> GetLanguages();
        void Compile(string source, string[] inputs);
    }
}