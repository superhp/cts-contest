using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Communication
{
    public interface IPrizeManager
    {
        Task<List<PrizeDto>> GetAllPrizes();
        Task<PrizeDto> GetPrizeById(int id);
    }
}