using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Db.Repository
{
    public interface IPrizeRepository
    {
        List<PrizeDto> GetAllWinnablePrizes();
        List<PrizeDto> GetAllPrizesForPoints();
        PrizeDto GetPrizeById(int id);
    }
}

