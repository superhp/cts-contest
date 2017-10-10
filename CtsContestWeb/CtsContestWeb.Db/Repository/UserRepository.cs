using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CtsContestWeb.Db.Entities;

namespace CtsContestWeb.Db.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void InsertIfNotExists(ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email).Value;
            
            var exists = _dbContext.Users.Any(u => u.Email.Equals(email));

            if (!exists)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;

                var picture = "";
                var provider = user.FindFirst(ClaimTypes.Actor).Value;
                if (provider.Equals("facebook"))
                    picture = $"https://graph.facebook.com/v2.10/{userId}/picture";

                var userEntity = new User
                {
                    Email = email,
                    FullName = user.FindFirst(ClaimTypes.GivenName).Value + " " + user.FindFirst(ClaimTypes.Surname).Value,
                    Picture = picture
                };

                _dbContext.Add(userEntity);
                _dbContext.SaveChanges();
            }
        }
    }
}
