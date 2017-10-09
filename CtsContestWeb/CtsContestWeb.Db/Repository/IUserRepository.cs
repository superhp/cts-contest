using System.Security.Claims;

namespace CtsContestWeb.Db.Repository
{
    public interface IUserRepository
    {
        void InsertIfNotExists(ClaimsPrincipal user);
    }
}