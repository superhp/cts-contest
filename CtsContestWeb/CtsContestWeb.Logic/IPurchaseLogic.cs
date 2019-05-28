using System.Threading.Tasks;

namespace CtsContestWeb.Logic
{
    public interface IPurchaseLogic
    {
        bool CheckIfUserCanBuy(string userEmail, int prizeId);
    }
}