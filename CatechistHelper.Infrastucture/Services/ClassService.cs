using CatechistHelper.API.Utils;
using CatechistHelper.Application.Extensions;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Class;
using CatechistHelper.Domain.Dtos.Requests.Timetable;
using CatechistHelper.Domain.Dtos.Responses.CatechistInClass;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Dtos.Responses.Slot;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Enums;
using CatechistHelper.Domain.Models;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Utils;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Linq.Expressions;

namespace CatechistHelper.Infrastructure.Services
{
    public class ClassService : BaseService<ClassService>, IClassService
    {

        private readonly ITimetableService _timetableService;
        public ClassService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<ClassService> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, ITimetableService timetableService)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _timetableService = timetableService;
        }

        public async Task<PagingResult<GetClassResponse>> GetPagination(ClassFilter? filter, int page, int size)
        {
            try
            {
                var result = await GetAll(filter, page, size);
                return SuccessWithPaging(
                            result.Adapt<IPaginate<GetClassResponse>>(),
                            page,
                            size,
                            result.Total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paginated class data with filter {Filter}, page {Page}, and size {Size}.", filter, page, size);
                return null!;
            }
        }

        public async Task<IPaginate<Class>> GetAll(ClassFilter? filter, int page, int size)
        {
            var classes = await _unitOfWork.GetRepository<Class>()
                    .GetPagingListAsync(
                            predicate: BuildGetPaginationQuery(filter),
                            orderBy: a => a.OrderByDescending(x => x.CreatedAt),
                            page: page,
                            size: size,
                            include: a => a.Include(p => p.PastoralYear)
                                           .Include(p => p.Grade)
                                           .Include(p => p.Grade.Major)
                    );

            return classes;
        }

        public async Task<PagingResult<GetCatechistInClassResponse>> GetCatechistInClassById(Guid id, int page, int size)
        {
            try
            {
                IPaginate<CatechistInClass> catechists =
                    await _unitOfWork.GetRepository<CatechistInClass>()
                    .GetPagingListAsync(
                            predicate: c => c.ClassId.Equals(id),
                            include: c => c.Include(c => c.Catechist),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        catechists.Adapt<IPaginate<GetCatechistInClassResponse>>(),
                        page,
                        size,
                        catechists.Total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching catechists for class ID {ClassId} with page {Page} and size {Size}.", id, page, size);
                return null!;
            }
        }

        private Expression<Func<Class, bool>> BuildGetPaginationQuery(ClassFilter? filter)
        {
            Expression<Func<Class, bool>> filterQuery = x => !x.IsDeleted;
            if (filter.MajorId != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Grade.Major.Id.Equals(filter.MajorId));
            }
            if (filter.GradeId != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.GradeId.Equals(filter.GradeId));
            }
            if (filter.PastoralYearId != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.PastoralYearId.Equals(filter.PastoralYearId));
            }
            return filterQuery;
        }

        public async Task<PagingResult<GetSlotResponse>> GetSlotsByClassId(Guid id, int page, int size)
        {
            try
            {
                IPaginate<Slot> slots =
                    await _unitOfWork.GetRepository<Slot>()
                    .GetPagingListAsync(
                            predicate: s => s.ClassId.Equals(id),
                            include: s => s.Include(s => s.Room)
                                           .Include(s => s.CatechistInSlots).ThenInclude(cis => cis.Catechist),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        slots.Adapt<IPaginate<GetSlotResponse>>(),
                        page,
                        size,
                        slots.Total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching slots for class ID {ClassId} with page {Page} and size {Size}.", id, page, size);
                return null!;
            }
        }

        private async Task CheckRestrictionDateAsync()
        {
            var configEntry = await _unitOfWork.GetRepository<SystemConfiguration>()
                .SingleOrDefaultAsync(predicate: sc => sc.Key == EnumUtil.GetDescriptionFromEnum(SystemConfigurationEnum.RestrictedDateManagingCatechism));

            if (configEntry == null || !DateTime.TryParseExact(configEntry.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var restrictionDate))
            {
                throw new Exception(MessageConstant.Common.RestrictedDateManagingCatechism);
            }

            if (DateTime.Now >= restrictionDate)
            {
                throw new Exception(MessageConstant.Common.RestrictedDateManagingCatechism);
            }
        }

        public async Task<Result<bool>> UpdateCatechistInClass(Guid id, CatechistInClassRequest classRequest)
        {
            try
            {
                await CheckRestrictionDateAsync();

                var classToUpdate = await GetClassByIdWithRelatedDataAsync(id);
                if (classToUpdate == null)
                {
                    return Fail<bool>(MessageConstant.Class.Fail.NotFoundClass);
                }

                await RemoveExistingCatechistDataAsync(classToUpdate);
                await AddNewCatechistInClassesAsync(id, classRequest.Catechists, classToUpdate);
                await AddNewCatechistInSlotsAsync(classRequest.Catechists, classToUpdate.Slots);

                await UpdateClassEntityAsync(classToUpdate);

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(MessageConstant.Class.Fail.UpdateClass);
            }
        }

        private async Task<Class> GetClassByIdWithRelatedDataAsync(Guid id)
        {
            return await _unitOfWork.GetRepository<Class>()
                .SingleOrDefaultAsync(
                    predicate: c => c.Id == id,
                    include: c => c.Include(c => c.CatechistInClasses)
                                   .Include(c => c.Slots)
                                   .ThenInclude(s => s.CatechistInSlots)
                );
        }

        private async Task RemoveExistingCatechistDataAsync(Class classToUpdate)
        {
            if (classToUpdate.CatechistInClasses != null)
            {
                _unitOfWork.GetRepository<CatechistInClass>().DeleteRangeAsync(classToUpdate.CatechistInClasses);
            }

            foreach (var slot in classToUpdate.Slots)
            {
                if (slot.CatechistInSlots != null && slot.Date >= DateTime.Now)
                {
                    _unitOfWork.GetRepository<CatechistInSlot>().DeleteRangeAsync(slot.CatechistInSlots);
                }
            }
        }

        private async Task AddNewCatechistInClassesAsync(Guid classId, IEnumerable<CatechistSlot> catechistSlots, Class classToUpdate)
        {
            foreach (var catechistSlot in catechistSlots)
            {
                var newCatechistInClass = new CatechistInClass
                {
                    ClassId = classId,
                    CatechistId = catechistSlot.CatechistId,
                    IsMain = catechistSlot.IsMain
                };

                classToUpdate.CatechistInClasses?.Add(newCatechistInClass);
                await _unitOfWork.GetRepository<CatechistInClass>().InsertAsync(newCatechistInClass);
            }
        }

        private async Task AddNewCatechistInSlotsAsync(IEnumerable<CatechistSlot> catechistSlots, IEnumerable<Slot> slots)
        {
            foreach (var slot in slots)
            {
                if (slot.Date >= DateTime.Now)
                {
                    foreach (var catechistSlot in catechistSlots)
                    {
                        var newCatechistInSlot = new CatechistInSlot
                        {
                            SlotId = slot.Id,
                            CatechistId = catechistSlot.CatechistId,
                            Type = catechistSlot.IsMain ? CatechistInSlotType.Main.ToString() : CatechistInSlotType.Assistant.ToString()
                        };

                        slot.CatechistInSlots.Add(newCatechistInSlot);
                        await _unitOfWork.GetRepository<CatechistInSlot>().InsertAsync(newCatechistInSlot);
                    }
                }
            }
        }

        private async Task UpdateClassEntityAsync(Class classToUpdate)
        {
            _unitOfWork.GetRepository<Class>().UpdateAsync(classToUpdate);
        }


        public async Task<Result<bool>> UpdateClassRoom(Guid id, RoomOfClassRequest request)
        {
            try
            {
                await CheckRestrictionDateAsync();

                // Retrieve the class along with related data
                var classToUpdate = await _unitOfWork.GetRepository<Class>()
                    .SingleOrDefaultAsync(
                        predicate: c => c.Id == id,
                        include: c => c.Include(c => c.Slots)
                                       .ThenInclude(s => s.CatechistInSlots)
                    );

                if (classToUpdate == null)
                {
                    return Fail<bool>(MessageConstant.Class.Fail.NotFoundClass);
                }

                foreach (var slot in classToUpdate.Slots)
                {
                    slot.RoomId = request.RoomId;
                }

                _unitOfWork.GetRepository<Class>().UpdateAsync(classToUpdate);

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(MessageConstant.Class.Fail.UpdateClass);
            }
        }

        public async Task<Result<bool>> CreateClass(ClassRequest request)
        {
            try
            {
                await CheckRestrictionDateAsync();
                var pastoralYear = await _unitOfWork.GetRepository<PastoralYear>().SingleOrDefaultAsync(predicate: y => y.Id == request.PastoralYearId);

                Validator.EnsureNonNull(pastoralYear);

                if (pastoralYear.PastoralYearStatus == PastoralYearStatus.Finished)
                {
                    throw new Exception("Năm học này đã kết thúc, không thể thêm mới");
                }

                var grade = await _unitOfWork.GetRepository<Grade>().SingleOrDefaultAsync(predicate: g => g.Id == request.GradeId);

                Validator.EnsureNonNull(grade);

                string[] years = pastoralYear.Name.Split('-');
                var startDate = await _timetableService.GetStartDate(years[0]);
                var endDate = await _timetableService.GetEndDate(years[1]);

                var classEntity = request.Adapt<Class>();
                classEntity.StartDate = startDate;
                classEntity.EndDate = endDate;

                await _unitOfWork.GetRepository<Class>().InsertAsync(classEntity);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(MessageConstant.Class.Fail.CreateClass);
            }
        }

        public async Task<Result<bool>> UpdateClass(Guid id, ClassRequest request)
        {
            try
            {
                var classEntity = await _unitOfWork.GetRepository<Class>().SingleOrDefaultAsync(predicate: c => c.Id == id);

                request.Adapt(classEntity);

                _unitOfWork.GetRepository<Class>().UpdateAsync(classEntity);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the class with ID {ClassId}.", id);
                return Fail<bool>(MessageConstant.Class.Fail.UpdateClass);
            }
        }

        public async Task<Result<bool>> DeleteClass(Guid id)
        {
            try
            {
                var classToDelete = await _unitOfWork.GetRepository<Class>()
                    .SingleOrDefaultAsync(predicate: c => c.Id == id, include: c => c.Include(cls => cls.CatechistInClasses)
                                                                                  .Include(cls => cls.Slots));
                if (classToDelete == null)
                {
                    return BadRequest<bool>("Không tìm thấy lớp này.");
                }

                if (classToDelete.CatechistInClasses.Count != 0)
                {
                    _unitOfWork.GetRepository<CatechistInClass>().DeleteRangeAsync(classToDelete.CatechistInClasses);
                }

                foreach (var slot in classToDelete.Slots)
                {
                    var catechistInSlots = await _unitOfWork.GetRepository<CatechistInSlot>()
                        .GetListAsync(predicate: cis => cis.SlotId == slot.Id);

                    if (slot.Date > DateTime.Now)
                    {
                        return BadRequest<bool>("Không thể xóa vì đã có slot.");
                    }

                    if (catechistInSlots.Count != 0)
                    {
                        _unitOfWork.GetRepository<CatechistInSlot>().DeleteRangeAsync(catechistInSlots);
                    }
                }

                if (classToDelete.Slots.Count != 0)
                {
                    _unitOfWork.GetRepository<Slot>().DeleteRangeAsync(classToDelete.Slots);
                }

                _unitOfWork.GetRepository<Class>().DeleteAsync(classToDelete);
                var result = await _unitOfWork.CommitAsync();

                return Success(result > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }


    }
}
