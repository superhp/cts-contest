using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CtsContestWeb.Communication;
using CtsContestWeb.Db;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Logic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CtsContestWeb.DI
{
    public static class TypeRegistrations
    {
        public static IContainer Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
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

            services.AddSingleton<IConfiguration>(configuration);

            var builder = new ContainerBuilder();
            builder.Populate(services);

            if (configuration["Compiler"].Equals("HackerRank"))
            {
                builder.RegisterType<HackerRankCompiler>().As<ICompiler>();
            }
            else
            {
                builder.RegisterType<PaizaCompiler>().As<ICompiler>();
            }

            builder.RegisterType<HackerRankCompiler>().Keyed<ICompiler>("HackerRank");
            builder.RegisterType<PaizaCompiler>().Keyed<ICompiler>("Paiza");

            return builder.Build();
        }
    }
}