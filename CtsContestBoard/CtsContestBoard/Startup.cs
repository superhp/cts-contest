using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using CtsContestBoard.Communications;
using CtsContestBoard.Db;
using CtsContestBoard.Db.Repository;
using DotNetify;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CtsContestBoard
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSignalR();
            services.AddDotNetify();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IPurchaseRepository, PurchaseRepository>();
            services.AddTransient<ISolutionRepository, SolutionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDuelRepository, DuelRepository>();

            services.AddTransient<IPrizeManager, PrizeManager>();

            services.AddSingleton<IConfiguration>(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseWebpackDevMiddleware();
            app.UseStaticFiles();

            app.UseWebSockets();
            app.UseSignalR();
            app.UseDotNetify(config => config.UseDeveloperLogging());

            app.Run(async (context) =>
            {
                using (var reader = new StreamReader(File.OpenRead("wwwroot/index.html")))
                    await context.Response.WriteAsync(reader.ReadToEnd());
            });
        }
    }
}