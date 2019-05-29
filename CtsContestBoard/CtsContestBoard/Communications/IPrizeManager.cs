using System.Collections.Generic;
using System.Threading.Tasks;
using CtsContestBoard.Dto;

namespace CtsContestBoard.Communications
{
    public interface IPrizeManager
    {
        List<PrizeDto> GetAllPrizes();
        PrizeDto GetPrizeById(int id);
    }
}