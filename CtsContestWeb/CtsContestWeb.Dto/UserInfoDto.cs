using System;
using System.Collections.Generic;
using System.Text;

namespace CtsContestWeb.Dto
{
    public class UserInfoDto
    {
        public bool IsLoggedIn { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
