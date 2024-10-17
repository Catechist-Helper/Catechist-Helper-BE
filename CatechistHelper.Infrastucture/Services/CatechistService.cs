using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Catechist;
using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CatechistHelper.Infrastructure.Services
{
    public class CatechistService : BaseService<CatechistService>, ICatechistService
    {
        public CatechistService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<CatechistService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        ) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetCatechistResponse>> Create(CreateCatechistRequest request)
        {
            try
            {
                var result = await CreateAsync(request);
                return Success(result.Adapt<GetCatechistResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetCatechistResponse>(ex.Message);
            }
        }

        public async Task<Catechist> CreateAsync(CreateCatechistRequest request)
        {
            var catechist = request.Adapt<Catechist>();

            var result = await _unitOfWork.GetRepository<Catechist>().InsertAsync(catechist);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception(MessageConstant.Catechist.Fail.CreateCatechist);
            }
            return result;
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                var catechist = await GetById(id);

                catechist.IsDeleted = true;

                _unitOfWork.GetRepository<Catechist>().UpdateAsync(catechist);
                return Success(await _unitOfWork.CommitAsync() > 0);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<GetCatechistResponse>> Get(Guid id)
        {
            var result = await GetById(id);
            return Success(result.Adapt<GetCatechistResponse>());
        }

        public async Task<Catechist> GetById(Guid id)
        {
            var catechist = await _unitOfWork.GetRepository<Catechist>()
                .SingleOrDefaultAsync(predicate: c => c.Id.Equals(id) && !c.IsDeleted);
            ArgumentNullException.ThrowIfNull(nameof(id));
            return catechist;
        }

        public async Task<IPaginate<Catechist>> GetAll(int page, int size, string? sortBy = null)
        {
            var catechists = await _unitOfWork.GetRepository<Catechist>()
                    .GetPagingListAsync(
                            predicate: a => !a.IsDeleted,
                            orderBy: a => a.OrderByDescending(x => x.CreatedAt),
                            page: page,
                            size: size
                    );

            return catechists;
        }

        public async Task<PagingResult<GetCatechistResponse>> GetPagination(Expression<Func<Catechist, bool>>? predicate, int page, int size)
        {
            try
            {
                var result = await GetAll(page, size);
                return SuccessWithPaging(
                            result.Adapt<IPaginate<GetCatechistResponse>>(),
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

        public async Task<Result<bool>> Update(Guid id, UpdateCatechistRequest request)
        {
            try
            {
                var catechist = await GetById(id);

                request.Adapt(catechist);
                _unitOfWork.GetRepository<Catechist>().UpdateAsync(catechist);

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Catechist.Fail.UpdateCatechist);
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
