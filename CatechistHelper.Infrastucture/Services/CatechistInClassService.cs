using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CatechistInClass;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    internal class CatechistInClassService : BaseService<CatechistInClassService>, ICatechistInClassService
    {
        public CatechistInClassService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<CatechistInClassService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<bool>> Create(CreateCatechistInClassRequest request)
        {
            try
            {
                Class classs = await _unitOfWork.GetRepository<Class>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(request.ClassId)) ?? throw new Exception(MessageConstant.Class.Fail.NotFoundClass);

                Catechist catechist = await _unitOfWork.GetRepository<Catechist>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(request.CatechistId)) ?? throw new Exception(MessageConstant.Catechist.Fail.NotFoundCatechist);

                CatechistInClass catechistInClass = request.Adapt<CatechistInClass>();
                CatechistInClass result = await _unitOfWork.GetRepository<CatechistInClass>().InsertAsync(catechistInClass);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.CatechistInClass.Fail.CreateCatechistInClass);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<bool>> Update(Guid id, UpdateCatechistInClassRequest request)
        {
            try
            {

                CatechistInClass classs = await _unitOfWork.GetRepository<CatechistInClass>().SingleOrDefaultAsync(
                predicate: c => c.ClassId.Equals(id)) ?? throw new Exception(MessageConstant.Class.Fail.NotFoundClass);

                Catechist catechist = await _unitOfWork.GetRepository<Catechist>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(request.CatechistId)) ?? throw new Exception(MessageConstant.Catechist.Fail.NotFoundCatechist);

                request.Adapt(classs);

                _unitOfWork.GetRepository<CatechistInClass>().UpdateAsync(classs);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.CatechistInClass.Fail.UpdateCatechistInClass);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }
    }
}
