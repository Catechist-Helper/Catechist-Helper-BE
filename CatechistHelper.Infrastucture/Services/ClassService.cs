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
        public ClassService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<ClassService> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
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
                Console.WriteLine(ex.Message);
            }
            return null!;
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
            }
            return null!;
        }

        private Expression<Func<Class, bool>> BuildGetPaginationQuery(ClassFilter? filter)
        {
            Expression<Func<Class, bool>> filterQuery = x => x.IsDeleted == false;
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
            }
            return null!;
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
                if (!isSuccessful)
                {
                    throw new Exception("Commit failed");
                }

                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(MessageConstant.Class.Fail.UpdateClass + ex.Message);
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
                if (slot.CatechistInSlots != null)
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

                // Update the class entity
                _unitOfWork.GetRepository<Class>().UpdateAsync(classToUpdate);

                // Save changes
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

                if (!isSuccessful)
                {
                    throw new Exception("Commit fail");
                }

                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(MessageConstant.Class.Fail.UpdateClass + ex.Message);
            }
        }
    }
}
