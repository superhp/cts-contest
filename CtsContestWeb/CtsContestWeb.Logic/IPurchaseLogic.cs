using System.Threading.Tasks;

namespace CtsContestWeb.Logic
{
    public interface IPurchaseLogic
    {
        Task<bool> CheckIfUserCanBuy(string userEmail, int prizeId);
    }
}