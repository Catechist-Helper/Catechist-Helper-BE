using Autofac;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Domain.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CatechistHelper.API.Configurations
{
    public static class ConfigureDbContext
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(AppConfig.ConnectionString.DefaultConnection));
            return services;
        }

        public static ContainerBuilder AddDbContext(this ContainerBuilder builder)
        {
            builder.Register(c => new SqlConnection(AppConfig.ConnectionString.DefaultConnection))
                    .As<IDbConnection>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().As<DbContext>().InstancePerLifetimeScope();
            return builder;
        }
    }
}
