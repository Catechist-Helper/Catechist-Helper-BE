using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.SystemConfiguration;
using CatechistHelper.Domain.Dtos.Responses.SystemConfiguration;
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
    public class SystemConfigurationService : BaseService<SystemConfigurationService>, ISystemConfiguration
    {
        public SystemConfigurationService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<SystemConfigurationService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetSystemConfigurationResponse>> Get(Guid id)
        {
            try
            {
                SystemConfiguration systemConfiguration = await _unitOfWork.GetRepository<SystemConfiguration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                return Success(systemConfiguration.Adapt<GetSystemConfigurationResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetSystemConfigurationResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetSystemConfigurationResponse>> GetPagination(Expression<Func<SystemConfiguration, bool>>? predicate, int page, int size)
        {
            try
            {

                IPaginate<SystemConfiguration> systemConfigurations =
                    await _unitOfWork.GetRepository<SystemConfiguration>()
                    .GetPagingListAsync(
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        systemConfigurations.Adapt<IPaginate<GetSystemConfigurationResponse>>(),
                        page,
                        size,
                        systemConfigurations.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<Result<GetSystemConfigurationResponse>> Create(CreateSystemConfigurationRequest request)
        {
            try
            {
                SystemConfiguration systemConfiguration = request.Adapt<SystemConfiguration>();

                SystemConfiguration result = await _unitOfWork.GetRepository<SystemConfiguration>().InsertAsync(systemConfiguration);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.SystemConfiguration.Fail.CreateSystemConfiguration);
                }
                return Success(result.Adapt<GetSystemConfigurationResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetSystemConfigurationResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Update(Guid id, UpdateSystemConfigurationRequest request)
        {
            try
            {
                SystemConfiguration systemConfiguration = await _unitOfWork.GetRepository<SystemConfiguration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.SystemConfiguration.Fail.NotFoundSystemConfiguration);

                request.Adapt(systemConfiguration);

                _unitOfWork.GetRepository<SystemConfiguration>().UpdateAsync(systemConfiguration);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.ChristianName.Fail.UpdateChristianName);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                SystemConfiguration systemConfiguration = await _unitOfWork.GetRepository<SystemConfiguration>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.SystemConfiguration.Fail.NotFoundSystemConfiguration);
                _unitOfWork.GetRepository<SystemConfiguration>().DeleteAsync(systemConfiguration);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.SystemConfiguration.Fail.DeleteSystemConfiguration);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<SystemConfiguration> GetConfigByKey(string key)
        {
            return await _unitOfWork.GetRepository<SystemConfiguration>()
                .SingleOrDefaultAsync(predicate: a => a.Key == key);
        }
    }
}
