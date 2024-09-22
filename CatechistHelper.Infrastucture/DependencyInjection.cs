using Autofac;
using CatechistHelper.Infrastructure.Repositories;
using CatechistHelper.Domain.Common;
using CatechistHelper.Application.Repositories;
using Mapster;
using MapsterMapper;
using System.Reflection;

namespace CatechistHelper.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
        public static void RegisterRepository(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(GenericRepository<>))
                    .As(typeof(IGenericRepository<>)).InstancePerDependency();

            builder.RegisterGeneric(typeof(UnitOfWork<>))
                .As(typeof(IUnitOfWork<>)).InstancePerDependency();

        }
        public static void RegisterMapster(this ContainerBuilder builder)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Default.IgnoreNullValues(true);

            var assemblies = new Assembly[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(BaseEntity).Assembly
            };

            config = config.ConfigCustomMapper();

            config.Scan(assemblies);

            builder.RegisterInstance(config).SingleInstance();
            builder.RegisterType<ServiceMapper>().As<IMapper>().InstancePerLifetimeScope();
        }
        private static TypeAdapterConfig ConfigCustomMapper(this TypeAdapterConfig config)
        {
            return config;
        }
    }
}
