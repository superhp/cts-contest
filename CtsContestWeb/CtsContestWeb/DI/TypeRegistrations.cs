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

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<ISolutionRepository, SolutionRepository>();
            services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
            services.AddScoped<IDuelRepository, DuelRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBalanceLogic, BalanceLogic>();
            services.AddScoped<IPurchaseLogic, PurchaseLogic>();
            services.AddScoped<ISolutionLogic, SolutionLogic>();
            services.AddScoped<IDuelLogic, DuelLogic>();

            services.AddTransient<IPrizeRepository, PrizeRepository>();
            services.AddTransient<ICodeSkeletonRepository, CodeSkeletonRepository>();

            services.AddTransient<ICodeSkeletonManager, CodeSkeletonManager>();
            services.AddSingleton<ITaskManager, TaskManager>();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton<ICompiler, HackerRankCompiler>();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterType<HackerRankCompiler>().Keyed<ICompiler>("HackerRank");
            builder.RegisterType<PaizaCompiler>().Keyed<ICompiler>("Paiza");
            builder.RegisterType<IdeoneCompiler>().Keyed<ICompiler>("Ideone");

            return builder.Build();
        }
    }
}