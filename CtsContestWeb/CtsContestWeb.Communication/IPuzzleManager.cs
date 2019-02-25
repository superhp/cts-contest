using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Communication
{
    public interface IPuzzleManager
    {
        Task<IEnumerable<PuzzleDto>> GetPuzzles(string userEmail = null);
        Task<PuzzleDto> GetPuzzle(int id);
    }
}
