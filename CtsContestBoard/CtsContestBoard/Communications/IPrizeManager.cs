using System.Collections.Generic;
using System.Threading.Tasks;
using CtsContestBoard.Dto;

namespace CtsContestBoard.Communications
{
    public interface IPrizeManager
    {
        Task<List<PrizeDto>> GetAllPrizes();
        Task<PrizeDto> GetPrizeById(int id);
    }
}