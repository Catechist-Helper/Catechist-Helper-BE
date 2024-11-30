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
using CatechistHelper.Domain.Dtos.Requests.Room;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Domain.Dtos.Responses.CatechistInSlot;
using CatechistHelper.Domain.Dtos.Requests.BudgetTransaction;
using CatechistHelper.Domain.Dtos.Requests.Interview;
using CatechistHelper.Domain.Dtos.Responses.AbsenceRequest;

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
            config.NewConfig<CreateRoomRequest, Room>()
                .Ignore(x => x.Image);
            config.NewConfig<UpdateRoomRequest, Room>()
                .Ignore(x => x.Image);

            config.NewConfig<Class, GetClassResponse>()
                .Map(dest => dest.PastoralYearName, src => src.PastoralYear.Name)
                .Map(dest => dest.GradeId, src => src.Grade.Id)
                .Map(dest => dest.GradeName, src => src.Grade.Name)
                .Map(dest => dest.MajorId, src => src.Grade.Major.Id)
                .Map(dest => dest.MajorName, src => src.Grade.Major.Name);

            config.NewConfig<Catechist, GetCatechistResponse>()
                .Map(dest => dest.ChristianName, src => src.ChristianName.Name)
                .Map(dest => dest.LevelName, src => src.Level.Name)
                .Map(dest => dest.Email, src => src.Account.Email)
                .Map(dest => dest.Account, src => src.Account)
                .Map(dest => dest.Certificates, src => src.Certificates)
                .Map(dest => dest.Level, src => src.Level);

            config.NewConfig<AbsenceRequest, Domain.Dtos.Responses.AbsenceRequest.GetAbsentRequest>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.CatechistId, src => src.CatechistId)
                .Map(dest => dest.SlotId, src => src.SlotId)
                .Map(dest => dest.Reason, src => src.Reason)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.ReplacementCatechistId, src => src.ReplacementCatechistId)
                .Map(dest => dest.ApproverId, src => src.ApproverId)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.ApprovalDate, src => src.ApprovalDate)
                .Map(dest => dest.CreateAt, src => src.CreatedAt)
                .Map(dest => dest.UpdateAt, src => src.UpdatedAt)
                .Map(dest => dest.CatechistName, src => src.Catechist.FullName)
                .Map(dest => dest.ReplacementCatechistName, src => src.ReplacementCatechist != null ? src.ReplacementCatechist.FullName : null)
                .Map(dest => dest.Approver, src => src.Approver != null ? src.Approver.FullName : null);

            config.NewConfig<Catechist, SearchCatechistResponse>()
            .Map(dest => dest.ChristianName, src => src.ChristianName != null ? src.ChristianName.Name : string.Empty) 
            .Map(dest => dest.Level, src => src.Level.Name) 
            .Map(dest => dest.Grade, src => src.CatechistInGrades.FirstOrDefault() != null ? src.CatechistInGrades.FirstOrDefault().Grade.Name : string.Empty) 
            .Map(dest => dest.Major, src => src.CatechistInGrades.FirstOrDefault() != null? src.CatechistInGrades.FirstOrDefault().Grade.Major.Name : string.Empty);

            config.NewConfig<CreateBudgetTransactionRequest, BudgetTransaction>()
                .Ignore(x => x.TransactionImages);
            config.NewConfig<CreateInterviewRequest, Interview>()
                .Ignore(x => x.Accounts);
            config.NewConfig<UpdateInterviewRequest, Interview>()
                .Ignore(x => x.Accounts);

            return config;
        }
    }
}
