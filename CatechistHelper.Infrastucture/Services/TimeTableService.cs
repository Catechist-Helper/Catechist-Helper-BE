using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Dtos;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Utils;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace CatechistHelper.Infrastructure.Services
{
    public class TimeTableService : BaseService<TimeTableService>
    {
        private readonly IPastoralYearService _pastoralYearService;

        private readonly ISystemConfiguration _systemConfiguration;
        public TimeTableService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<TimeTableService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IPastoralYearService pastoralYearService,
            ISystemConfiguration systemConfiguration
        ) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _pastoralYearService = pastoralYearService;
            _systemConfiguration = systemConfiguration;
        }



        public async Task<List<Class>> CreateTimetable(IFormFile file)
        {
            var pastoralYearDto = FileHelper.ReadFile(file);
            var pastoralYear = await _pastoralYearService.Create(pastoralYearDto);

            string[] years = pastoralYear.Name.Split('-');
            var startDate = await GetStartDay(years[0]);
            var endDate = await GetEndDay(years[1]);

            var createdClasses = new List<Class>();

            foreach (var majorDto in pastoralYearDto.Majors)
            {
                var major = await _unitOfWork.GetRepository<Major>()
                    .SingleOrDefaultAsync(predicate: m => m.Name == majorDto.Name);

                foreach (var gradeDto in majorDto.Grades)
                {
                    var grade = await CreateGrade(gradeDto, pastoralYear.Id, major.Id);

                    foreach (var classDto in gradeDto.Classes)
                    {
                        classDto.StartDate = startDate;
                        classDto.EndDate = endDate;
                        var classEntity = await CreateClass(classDto, grade.Id, pastoralYear.Id);
                        createdClasses.Add(classEntity);
                    }
                }
            }

            await _unitOfWork.CommitAsync();

            return createdClasses;

        }


        private async Task<Grade> CreateGrade(GradeDto gradeDto, Guid pastoralYearId, Guid majorId)
        {
            var grade = await _unitOfWork.GetRepository<Grade>()
                .SingleOrDefaultAsync(predicate: g => g.Name == gradeDto.Name && g.PastoralYearId == pastoralYearId);

            if (grade == null)
            {
                grade = new Grade
                {
                    Name = gradeDto.Name,
                    MajorId = majorId,
                    PastoralYearId = pastoralYearId
                };

                await _unitOfWork.GetRepository<Grade>().InsertAsync(grade);
            }

            return grade;
        }

        private async Task<Class> CreateClass(ClassDto classDto, Guid gradeId, Guid pastoralYearId)
        {
            var classEntity = new Class
            {
                Name = classDto.Name,
                StartDate = classDto.StartDate,
                EndDate = classDto.EndDate,
                NumberOfCatechist = classDto.NumberOfCatechist,
                GradeId = gradeId,
                PastoralYearId = pastoralYearId
            };

            await _unitOfWork.GetRepository<Class>().InsertAsync(classEntity);
            return classEntity;
        }


        public async Task<DateTime> GetStartDay(string year)
        {
            var startDate = await _systemConfiguration.GetConfigByKey("startdate");
            return ConvertToDateTime(startDate.Value + "/" + year);
        }

        public async Task<DateTime> GetEndDay(string year)
        {
            var endDate = await _systemConfiguration.GetConfigByKey("enddate");
            return ConvertToDateTime(endDate.Value + "/" + year);
        }

        public static DateTime ConvertToDateTime(string date)
        {
            return DateTime.ParseExact(date, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture
                    );
        }
    }
}
