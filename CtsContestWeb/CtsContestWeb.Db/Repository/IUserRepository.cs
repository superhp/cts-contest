using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Db.Repository
{
    public interface IUserRepository
    {
        void InsertIfNotExists(ClaimsPrincipal user);
        IEnumerable<UserInfoDto> GetAllUsers();
    }
}