using System.Threading.Tasks;

namespace CtsContestWeb.Logic
{
    public interface IBalanceLogic
    {
        bool IsBalanceEnough(string userEmail, int prizeId);
        int GetCurrentBalance(string userEmail);
        int GetTotalBalance(string userEmail);
    }
}