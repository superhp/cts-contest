using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CtsContestWeb.Filters
{
    public class LoggedIn : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("Login before trying to access this resource");

            base.OnActionExecuting(context);
        }
    }
}
