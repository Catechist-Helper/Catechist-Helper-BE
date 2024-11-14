using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CatechistInClass;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
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

        
        
    }



    

    
}
