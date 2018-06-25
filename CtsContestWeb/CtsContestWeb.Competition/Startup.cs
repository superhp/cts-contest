using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CtsContestWeb.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CtsContestWeb.Competition
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
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(Configuration.GetValue<string>("Hostname"))
                    .AllowCredentials();
            }));

            services.AddSignalR();

            ApplicationContainer = TypeRegistrations.Register(services, Configuration);

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.Use(async (context, next) =>
                {
                    Authentication.GetDeveloperIdentity(context);

                    await next.Invoke();
                });
            }
            else
            {
                app.UseDeveloperExceptionPage();

                app.Use(async (context, next) =>
                {
                    await Authentication.GetAzureIdentity(context);

                    await next.Invoke();
                });
            }

            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<CompetitionHub>("/competition");
            });
        }
    }
}
