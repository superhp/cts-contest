using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Dto;
using Newtonsoft.Json;
using RestSharp;

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
                else if (provider.Equals("google"))
                {
                    var client = new RestClient("http://picasaweb.google.com");

                    var request = new RestRequest($"/data/entry/api/user/{email}?alt=json", Method.GET);

                    TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
                    client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

                    var response = taskCompletion.Task.Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var json = response.Content.Replace("$", "");
                        dynamic data = JsonConvert.DeserializeObject(json);

                        picture = data.entry.gphotothumbnail.t;
                    }
                }

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

        public IEnumerable<UserInfoDto> GetAllUsers()
        {
            return _dbContext.Users.Select(u => new UserInfoDto
            {
                Name = u.FullName,
                //TotalBalance = _dbContext.Competitions.Where(c => c.WinnerEmail.Equals(u.Email)).Sum(c => c.Prize)

                TotalBalance = u.Solutions.Where(s => s.IsCorrect).Sum(s => s.Score) - u.Purchases.Select(x => x.Cost).DefaultIfEmpty(0).Sum()
            });
        }
    }
}
