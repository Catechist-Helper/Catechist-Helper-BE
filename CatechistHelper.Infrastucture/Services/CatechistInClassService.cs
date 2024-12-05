using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CatechistInClass;
using CatechistHelper.Domain.Dtos.Responses.CatechistInSlot;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using LinqKit;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class CatechistInClassService : BaseService<CatechistInClassService>, ICatechistInClassService
    {

        public CatechistInClassService(
            IUnitOfWork<ApplicationDbContext> unitOfWork, 
            ILogger<CatechistInClassService> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor
            ) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
           
        }

        public async Task<Result<bool>> AddCatechistToClass(CreateCatechistInClassRequest request)
        {
            try
            {
                var classFromDb = await _unitOfWork.GetRepository<Class>().SingleOrDefaultAsync(
                    predicate: c => c.Id == request.ClassId,
                    include: c => c.Include(c => c.CatechistInClasses)
                ) ?? throw new Exception(MessageConstant.Class.Fail.NotFoundClass);
                var requestedCatechists = await _unitOfWork.GetRepository<Catechist>().GetListAsync(
                    predicate: c => request.CatechistIds.Contains(c.Id)
                );
                if (requestedCatechists.Count != request.CatechistIds.Count)
                {
                    throw new Exception(MessageConstant.Catechist.Fail.NotFoundCatechist);
                }
                // Remove catechists not in the request
                var catechistsToRemove = classFromDb.CatechistInClasses.Where(cic => !request.CatechistIds.Contains(cic.CatechistId)).ToList();
                if (catechistsToRemove.Any())
                {
                    _unitOfWork.GetRepository<CatechistInClass>().DeleteRangeAsync(catechistsToRemove);
                }
                foreach (var catechist in requestedCatechists)
                {
                    var isMain = catechist.Id == request.MainCatechistId;
                    var existingCatechistInClass = classFromDb.CatechistInClasses.FirstOrDefault(cic => cic.CatechistId == catechist.Id);
                    if (existingCatechistInClass != null)
                    {
                        if (existingCatechistInClass.IsMain != isMain)
                        {
                            existingCatechistInClass.IsMain = isMain;
                            _unitOfWork.GetRepository<CatechistInClass>().UpdateAsync(existingCatechistInClass);
                        }
                    }
                    else
                    {
                        await _unitOfWork.GetRepository<CatechistInClass>().InsertAsync(new CatechistInClass
                        {
                            ClassId = request.ClassId,
                            CatechistId = catechist.Id,
                            IsMain = isMain
                        });
                    }
                }
                await _unitOfWork.CommitAsync();
                return Success(true);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<PagingResult<SearchCatechistResponse>> SearchAvailableCatechists(Guid id, Guid excludeId, int page, int size)
        {
            try
            {
                // Step 1: Retrieve the CatechistInSlot for the excluded catechist (from the same slot)
                var catechistInClass = await _unitOfWork.GetRepository<CatechistInClass>()
                .SingleOrDefaultAsync(
                    predicate: s => s.ClassId == id && s.CatechistId == excludeId,
                    include: s => s.Include(c => c.Class)
                                   .Include(c => c.Catechist)
                                  .ThenInclude(c => c.CatechistInGrades)
                                  .ThenInclude(g => g.Grade)
                                  .ThenInclude(g => g.Major)
                );

                if (catechistInClass == null || catechistInClass.Catechist == null || catechistInClass.Class == null)
                {
                    throw new InvalidOperationException("The specified catechist does not exist or is not assigned to the class.");
                }

                // Step 2: Extract GradeId and MajorId from the excluded catechist
                var gradeId = catechistInClass.Catechist.CatechistInGrades.FirstOrDefault()?.GradeId;
                var majorId = catechistInClass.Catechist.CatechistInGrades.FirstOrDefault()?.Grade?.MajorId;

                // Step 3: Build the predicate with the conditions
                var predicate = PredicateBuilder.New<Catechist>()
                    .And(c => c.IsTeaching && !c.IsDeleted)  // Catechist must be teaching and not deleted
                    .And(c => c.CatechistInGrades.Any(g => g.GradeId == gradeId || g.Grade.MajorId == majorId)) // Same grade or major
                    .And(c => c.Id != excludeId)  // Exclude the current catechist
                    .And(c => !c.CatechistInClasses
                        .Any(cs => cs.IsMain));  // Exclude catechists with the same date

                IPaginate<Catechist> catechists = await _unitOfWork.GetRepository<Catechist>()
                    .GetPagingListAsync(
                        predicate: predicate,
                        include: s => s.Include(c => c.Level)
                                      .Include(c => c.ChristianName)
                                      .Include(c => c.CatechistInGrades)
                                      .ThenInclude(g => g.Grade)
                                      .ThenInclude(g => g.Major)
                                      .Include(c => c.CatechistInClasses),
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
