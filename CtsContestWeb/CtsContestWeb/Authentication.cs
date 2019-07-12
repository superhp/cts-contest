using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace CtsContestWeb
{
    public static class Authentication
    {
        public static void GetDeveloperIdentity(HttpContext context)
        {
            string emailHeaderVal = context.Request.Headers.FirstOrDefault(header => header.Key.Equals("email")).Value;
            // Create claims for testing/development
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Test Developer"),
                new Claim(ClaimTypes.NameIdentifier, "LocalDev"),
                new Claim(ClaimTypes.Email, emailHeaderVal ?? "LocalDev@local.com"),
                new Claim(ClaimTypes.Surname, "Developer"),
                new Claim(ClaimTypes.GivenName, "Test"),
                new Claim(ClaimTypes.Actor, "Test"),
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"),
                new Claim("AccessToken", "fake_token")
            };

            // Set user in current context as claims principal
            var identity = new GenericIdentity("Dev");
            identity.AddClaims(claims);

            // Set current thread user to identity
            context.User = new GenericPrincipal(identity, null);
        }

        public static async Task GetAzureIdentity(HttpContext context)
        {
            // Create a user on current thread from provided header
            if (context.User?.Identity == null || context.User.Identity.IsAuthenticated == false)
            {
                //invoke /.auth/me
                var cookieContainer = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer
                };
                string uriString = $"{context.Request.Scheme}://{context.Request.Host}";
                foreach (var c in context.Request.Cookies)
                {
                    cookieContainer.Add(new Uri(uriString), new Cookie(c.Key, c.Value));
                }
                using (HttpClient client = new HttpClient(handler))
                {
                    var res = await client.GetAsync($"{uriString}/.auth/me");
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonResult = await res.Content.ReadAsStringAsync();

                        //parse json
                        var obj = JArray.Parse(jsonResult);
                        string userId = obj[0]["user_id"].Value<string>(); //user_id
                        var provider = obj[0]["provider_name"].Value<string>();
                        string accessToken = obj[0]["access_token"].Value<string>();

                        // Create claims id
                        List<Claim> claims = new List<Claim>();
                        foreach (var claim in obj[0]["user_claims"])
                        {
                            claims.Add(new Claim(claim["typ"].ToString(), claim["val"].ToString()));
                        }
                        claims.Add(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
                        claims.Add(new Claim(ClaimTypes.Actor, provider));
                        claims.Add(new Claim("AccessToken", accessToken));
                        // Set user in current context as claims principal
                        var identity = new GenericIdentity(userId);
                        identity.AddClaims(claims);

                        // Set current thread user to identity
                        context.User = new GenericPrincipal(identity, null);
                    }
                }

            }
        }
    }
}