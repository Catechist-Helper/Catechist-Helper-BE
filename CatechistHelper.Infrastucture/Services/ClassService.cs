using CatechistHelper.API.Utils;
using CatechistHelper.Application.Extensions;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Class;
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

                // Retrieve the class along with related data
                var classToUpdate = await _unitOfWork.GetRepository<Class>()
                    .SingleOrDefaultAsync(
                        predicate: c => c.Id == id,
                        include: c => c.Include(c => c.CatechistInClasses)
                                       .Include(c => c.Slots)
                                       .ThenInclude(s => s.CatechistInSlots)
                    );

                if (classToUpdate == null)
                {
                    return Fail<bool>(MessageConstant.Class.Fail.NotFoundClass);
                }
                // Remove existing CatechistInClasses
                classToUpdate.CatechistInClasses.Clear();

                // Remove existing CatechistInSlots for all slots
                foreach (var slot in classToUpdate.Slots)
                {
                    slot.CatechistInSlots.Clear();
                }

                // Add new CatechistInClasses
                foreach (var catechistSlot in classRequest.Catechists)
                {
                    classToUpdate.CatechistInClasses.Add(new CatechistInClass
                    {
                        ClassId = id,
                        CatechistId = catechistSlot.CatechistId,
                        IsMain = catechistSlot.IsMain
                    });
                }

                // Add new CatechistInSlots
                foreach (var slot in classToUpdate.Slots)
                {
                    foreach (var catechistSlot in classRequest.Catechists)
                    {
                        slot.CatechistInSlots.Add(new CatechistInSlot
                        {
                            SlotId = slot.Id,
                            CatechistId = catechistSlot.CatechistId,
                            Type = catechistSlot.IsMain ? CatechistInSlotType.Main.ToString() : CatechistInSlotType.Assistant.ToString()
                        });
                    }
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
