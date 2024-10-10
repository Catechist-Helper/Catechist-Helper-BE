using Autofac;
using CatechistHelper.Infrastructure.Repositories;
using CatechistHelper.Domain.Common;
using CatechistHelper.Application.Repositories;
using Mapster;
using MapsterMapper;
using System.Reflection;
using Google.Cloud.Storage.V1;
using CatechistHelper.Domain.Dtos.Requests.Registration;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Dtos.Requests.Account;

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
            builder.Register(ctx =>
            {
                var storageClient = StorageClient.Create();
                return storageClient;
            }).SingleInstance();
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
            config.NewConfig<CreateRegistrationRequest, Registration>()
                .Ignore(x => x.CertificateOfCandidates);
            config.NewConfig<CreateAccountRequest, Account>()
                .Ignore(x => x.Avatar);
            config.NewConfig<UpdateAccountRequest, Account>()
                .Ignore(x => x.Avatar);
            return config;
        }
    }
}
