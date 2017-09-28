using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CtsContestWeb.Db.DataAccess;
using CtsContestWeb.Db;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Security.Claims;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System;
using Newtonsoft.Json.Linq;
using CtsContestWeb.Communication;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Middleware;
using CtsContestWeb.Logic;

namespace CtsContestWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<ISolutionRepository, SolutionRepository>();
            services.AddScoped<IBalanceLogic, BalanceLogic>();
            services.AddScoped<IPurchaseLogic, PurchaseLogic>();

            services.AddTransient<ITaskManager, TaskManager>();
            services.AddTransient<IPrizeManager, PrizeManager>();
            services.AddTransient<ICompiler, Compiler>();

            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });

                app.Use(async (context, next) =>
                {
                    // Create claims for testing/development
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "Test Developer"),
                        new Claim(ClaimTypes.NameIdentifier, "LocalDev"),
                        new Claim(ClaimTypes.Email, "LocalDev@local.com"),
                        new Claim(ClaimTypes.Surname, "Developer"),
                        new Claim(ClaimTypes.GivenName, "Test"),
                        new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity")
                    };

                    // Set user in current context as claims principal
                    var identity = new GenericIdentity("Dev");
                    identity.AddClaims(claims);

                    // Set current thread user to identity
                    context.User = new GenericPrincipal(identity, null);

                    await next.Invoke();
                });
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseDeveloperExceptionPage();

                app.Use(async (context, next) =>
                {
                    // Create a user on current thread from provided header
                    if (context.Request.Headers.ContainsKey("X-MS-CLIENT-PRINCIPAL-ID"))
                    {
                        // Read headers from Azure
                        var azureAppServicePrincipalIdHeader = context.Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"][0];
                        var azureAppServicePrincipalNameHeader = context.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"][0];

                        //invoke /.auth/me
                        var cookieContainer = new CookieContainer();
                        HttpClientHandler handler = new HttpClientHandler()
                        {
                            CookieContainer = cookieContainer
                        };
                        string uriString = $"{context.Request.Scheme}://{context.Request.Host}";
                        foreach (var c in context.Request.Cookies)
                        {
                            cookieContainer.Add(new Uri(uriString), new Cookie(c.Key, c.Value));
                        }
                        string jsonResult = string.Empty;
                        using (HttpClient client = new HttpClient(handler))
                        {
                            var res = await client.GetAsync($"{uriString}/.auth/me");
                            jsonResult = await res.Content.ReadAsStringAsync();
                        }

                        //parse json
                        var obj = JArray.Parse(jsonResult);
                        string user_id = obj[0]["user_id"].Value<string>(); //user_id

                        // Create claims id
                        List<Claim> claims = new List<Claim>();
                        foreach (var claim in obj[0]["user_claims"])
                        {
                            claims.Add(new Claim(claim["typ"].ToString(), claim["val"].ToString()));
                        }

                        // Set user in current context as claims principal
                        var identity = new GenericIdentity(azureAppServicePrincipalIdHeader);
                        identity.AddClaims(claims);

                        // Set current thread user to identity
                        context.User = new GenericPrincipal(identity, null);
                    }

                    await next.Invoke();
                });
            }

            // Automatic migrations 
            // TODO: move it to TSVS in future
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
            }



            app.UseStaticFiles();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
