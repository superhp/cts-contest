using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CtsContestWeb.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpGet("")]
        public UserInfoDto GetUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                return new UserInfoDto
                {
                    Email = User.FindFirst(ClaimTypes.Email).Value,
                    Name = User.FindFirst(ClaimTypes.GivenName).Value,
                    IsLoggedIn = true
                };
            }

            return new UserInfoDto
            {
                IsLoggedIn = false
            };
        }
    }
}