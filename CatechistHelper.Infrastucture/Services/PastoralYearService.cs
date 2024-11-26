using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.PastoralYear;
using CatechistHelper.Domain.Dtos.Responses.PastoralYear;
using CatechistHelper.Domain.Dtos.Responses.Timetable;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

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
                PastoralYear pastoralYear = await GetById(id);

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
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    // 547 is the SQL Server error code for a foreign key violation
                    return Fail<bool>(MessageConstant.Common.DeleteFail);
                }
                else
                {
                    return Fail<bool>(ex.Message);
                }
            }
        }

        public async Task<Result<GetPastoralYearResponse>> Get(Guid id)
        {
            try
            {
                PastoralYear pastoralYear = await GetById(id);

                return Success(pastoralYear.Adapt<GetPastoralYearResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetPastoralYearResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetPastoralYearResponse>> GetPagination(Expression<Func<PastoralYear, bool>>? predicate, int page, int size)
        {
            try
            {

                IPaginate<PastoralYear> pastoralYears =
                    await _unitOfWork.GetRepository<PastoralYear>()
                    .GetPagingListAsync(
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        pastoralYears.Adapt<IPaginate<GetPastoralYearResponse>>(),
                        page,
                        size,
                        pastoralYears.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
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

        public async Task<PastoralYear> Create(PastoralYearDto dto)
        {
            var pastoralYear = dto.Adapt<PastoralYear>();

            pastoralYear.PastoralYearStatus = Domain.Enums.PastoralYearStatus.Active;

            var result = await _unitOfWork.GetRepository<PastoralYear>().InsertAsync(pastoralYear);

            return result;
        }

        public async Task<PastoralYear> GetById(Guid id)
        {
            PastoralYear pastoralYear = await _unitOfWork.GetRepository<PastoralYear>().SingleOrDefaultAsync(
                   predicate: a => a.Id.Equals(id));
            return pastoralYear;
        }

        public async Task<PastoralYear> GetByName(string name)
        {
            PastoralYear pastoralYear = await _unitOfWork.GetRepository<PastoralYear>().SingleOrDefaultAsync(
                  predicate: a => a.Name.Equals(name));
            return pastoralYear;
        }
    }
}
