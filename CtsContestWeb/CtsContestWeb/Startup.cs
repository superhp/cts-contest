using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CtsContestWeb.Db;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Security.Claims;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using CtsContestWeb.Communication;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Logic;
using CtsContestWeb.Middleware;

namespace CtsContestWeb
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMemoryCache();

            services.AddDbContext<ApplicationDbContext>(options =>

                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            5,
                            TimeSpan.FromSeconds(1),
                            null);
                    }
                )
            );

            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<ISolutionRepository, SolutionRepository>();
            services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBalanceLogic, BalanceLogic>();
            services.AddScoped<IPurchaseLogic, PurchaseLogic>();

            services.AddTransient<ITaskManager, TaskManager>();
            services.AddTransient<IPrizeManager, PrizeManager>();
            services.AddTransient<ICodeSkeletonManager, CodeSkeletonManager>();

            services.AddScoped<ISolutionLogic, SolutionLogic>();

            services.AddSingleton<IConfiguration>(Configuration);

            var builder = new ContainerBuilder();
            builder.Populate(services);

            if (Configuration["Compiler"].Equals("HackerRank"))
            {
                builder.RegisterType<HackerRankCompiler>().As<ICompiler>();
            }
            else
            {
                builder.RegisterType<PaizaCompiler>().As<ICompiler>();
            }

            builder.RegisterType<HackerRankCompiler>().Keyed<ICompiler>("HackerRank");
            builder.RegisterType<PaizaCompiler>().Keyed<ICompiler>("Paiza");

            ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

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
                    Authentication.GetDeveloperIdentity(context);

                    await next.Invoke();
                });
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseDeveloperExceptionPage();

                app.Use(async (context, next) =>
                {
                    await Authentication.GetAzureIdentity(context);

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
