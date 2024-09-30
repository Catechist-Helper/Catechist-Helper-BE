using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.PastoralYear;
using CatechistHelper.Domain.Dtos.Responses.PastoralYear;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class PastoralYearService : BaseService<PastoralYearService>, IPastoralYearService
    {
        public PastoralYearService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<PastoralYearService> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetPastoralYearResponse>> Create(CreatePastoralYearRequest request)
        {
            try
            {

                PastoralYear pastoralYear = request.Adapt<PastoralYear>();

                PastoralYear result = await _unitOfWork.GetRepository<PastoralYear>().InsertAsync(pastoralYear);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.PastoralYear.Fail.CreatePastoralYear);
                }
                return Success(_mapper.Map<GetPastoralYearResponse>(result));
            }
            catch (Exception ex)
            {
                return Fail<GetPastoralYearResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                PastoralYear pastoralYear = await _unitOfWork.GetRepository<PastoralYear>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                _unitOfWork.GetRepository<PastoralYear>().DeleteAsync(pastoralYear);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.PastoralYear.Fail.DeletePastoralYear);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<GetPastoralYearResponse>> Get(Guid id)
        {
            try
            {
                PastoralYear pastoralYear = await _unitOfWork.GetRepository<PastoralYear>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                return Success(pastoralYear.Adapt<GetPastoralYearResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetPastoralYearResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Update(Guid id, UpdatePastoralYearRequest request)
        {
            try
            {
                PastoralYear pastoralYear = await _unitOfWork.GetRepository<PastoralYear>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                request.Adapt(pastoralYear);

                _unitOfWork.GetRepository<PastoralYear>().UpdateAsync(pastoralYear);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.PastoralYear.Fail.UpdatePastoralYear);
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
