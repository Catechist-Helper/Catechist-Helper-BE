using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Level;
using CatechistHelper.Domain.Dtos.Responses.Level;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace CatechistHelper.Infrastructure.Services
{
    public class LevelService : BaseService<LevelService>, ILevelService
    {
        public LevelService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<LevelService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetLevelResponse>> Create(CreateLevelRequest request)
        {
            try
            {
                var result = await CreateAsync(request);
                return Success(result.Adapt<GetLevelResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetLevelResponse>(ex.Message);
            }
        }

        public async Task<Level> CreateAsync(CreateLevelRequest request)
        {
            var level = request.Adapt<Level>();
            var result = await _unitOfWork.GetRepository<Level>().InsertAsync(level);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception(MessageConstant.Level.Fail.CreateLevel);
            }
            return result;
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                var catechist = await GetById(id);

                catechist.IsDeleted = true;

                _unitOfWork.GetRepository<Level>().UpdateAsync(catechist);
                return Success(await _unitOfWork.CommitAsync() > 0);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<GetLevelResponse>> Get(Guid id)
        {
            var result = await GetById(id);
            return Success(result.Adapt<GetLevelResponse>());
        }

        public async Task<Level> GetById(Guid id)
        {
            var level = await _unitOfWork.GetRepository<Level>()
                .SingleOrDefaultAsync(predicate: c => c.Id.Equals(id));
            ArgumentNullException.ThrowIfNull(nameof(id));
            return level;
        }

        public async Task<IPaginate<Level>> GetAllAsync()
        {
            return await _unitOfWork.GetRepository<Level>()
                     .GetPagingListAsync(
                            predicate: a => !a.IsDeleted,
                            orderBy: a => a.OrderBy(x => x.CatechismLevel)
                     );
        }
    }
}
