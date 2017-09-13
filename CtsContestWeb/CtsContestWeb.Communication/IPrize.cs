using CtsContestWeb.Dto;
using System.Collections.Generic;

namespace CtsContestWeb.Communication
{
    public interface IPrize
    {
        List<PrizeDto> GetAllPrizes();
        PrizeDto GetPrizeById(int id);
    }
}