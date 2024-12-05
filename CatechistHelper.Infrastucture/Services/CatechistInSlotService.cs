using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CatechistInSlot;
using CatechistHelper.Domain.Dtos.Responses.CatechistInSlot;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Enums;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Utils;
using LinqKit;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class CatechistInSlotService : BaseService<CatechistInSlotService>, ICatechistInSlotService
    {
        public CatechistInSlotService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<CatechistInSlotService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<bool>> ReplaceCatechist(Guid id, ReplaceCatechistRequest request)
        {
            try
            {
                var slot = await _unitOfWork.GetRepository<Slot>()
                    .SingleOrDefaultAsync(
                        predicate: s => s.Id == id,
                        include: s => s.Include(s => s.CatechistInSlots)
                    );

                Validator.EnsureNonNull(slot);

                var catechistToReplace = slot.CatechistInSlots
                    .FirstOrDefault(c => c.CatechistId == request.CurrentId);

                Validator.EnsureNonNull(catechistToReplace);

                slot.CatechistInSlots.Remove(catechistToReplace);
                _unitOfWork.GetRepository<CatechistInSlot>().DeleteAsync(catechistToReplace);

                var newCatechistInSlot = new CatechistInSlot
                {
                    CatechistId = request.ReplaceId,
                    Type = request.Type.ToString(),
                    SlotId = id
                };

                await _unitOfWork.GetRepository<CatechistInSlot>().InsertAsync(newCatechistInSlot);
                slot.CatechistInSlots.Add(newCatechistInSlot);

                _unitOfWork.GetRepository<Slot>().UpdateAsync(slot);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

                return isSuccessful
                    ? Success(true)
                    : Fail<bool>(MessageConstant.CatechistInSlot.Fail.UpdateCatechistInSlot);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<PagingResult<SearchCatechistResponse>> SearchAvailableCatechists(Guid slotId, Guid excludeId, int page, int size)
        {
            try
            {
                // Step 1: Retrieve the CatechistInSlot for the excluded catechist (from the same slot)
                var catechistInSlot = await _unitOfWork.GetRepository<CatechistInSlot>()
                .SingleOrDefaultAsync(
                    predicate: s => s.SlotId == slotId && s.CatechistId == excludeId,
                    include: s => s.Include(catechistInSlot => catechistInSlot.Slot)
                                   .Include(c => c.Catechist)
                                  .ThenInclude(c => c.CatechistInGrades)
                                  .ThenInclude(g => g.Grade)
                                  .ThenInclude(g => g.Major)
                );

                if (catechistInSlot == null || catechistInSlot.Catechist == null || catechistInSlot.Slot == null)
                {
                    throw new InvalidOperationException("The specified catechist does not exist or is not assigned to the slot.");
                }

                // Step 2: Extract GradeId and MajorId from the excluded catechist
                var gradeId = catechistInSlot.Catechist.CatechistInGrades.FirstOrDefault()?.GradeId;
                var majorId = catechistInSlot.Catechist.CatechistInGrades.FirstOrDefault()?.Grade?.MajorId;
                var date = catechistInSlot.Slot.Date;

                // Step 3: Build the predicate with the conditions
                var predicate = PredicateBuilder.New<Catechist>()
                    .And(c => c.IsTeaching && !c.IsDeleted)  // Catechist must be teaching and not deleted
                    .And(c => c.CatechistInGrades.Any(g => g.GradeId == gradeId || g.Grade.MajorId == majorId)) // Same grade or major
                    .And(c => c.Id != excludeId)  // Exclude the current catechist
                    .And(c => !c.CatechistInSlots
                        .Any(cs => cs.Slot.Date == date && cs.Type == CatechistInSlotType.Main.ToString()));  // Exclude catechists with the same date

                IPaginate<Catechist> catechists = await _unitOfWork.GetRepository<Catechist>()
                    .GetPagingListAsync(
                        predicate: predicate,
                        include: s => s.Include(c => c.Level)
                                      .Include(c => c.ChristianName)
                                      .Include(c => c.CatechistInGrades)
                                      .ThenInclude(g => g.Grade)
                                      .ThenInclude(g => g.Major)
                                      .Include(c => c.CatechistInSlots)
                                      .ThenInclude(c => c.Slot),
                        page: page,
                        size: size
                    );

                var data = catechists.Adapt<IPaginate<SearchCatechistResponse>>();

                // Step 5: Return the paginated result
                return SuccessWithPaging(data, page, size, catechists.Total);
            }
            catch (Exception ex)
            {
                return NotFounds<SearchCatechistResponse>(ex.Message);
            }
            
        }


    }
}
