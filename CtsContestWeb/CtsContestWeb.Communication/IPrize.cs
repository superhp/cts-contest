using CtsContestWeb.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Communication
{
    public interface IPrize
    {
        Task<List<PrizeDto>> GetAllPrizes();
        Task<PrizeDto> GetPrizeById(int id);
    }
}