using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.SystemConfiguration;
using CatechistHelper.Domain.Dtos.Responses.SystemConfiguration;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface ISystemConfigurationService
    {
        Task<PagingResult<GetSystemConfigurationResponse>> GetPagination(Expression<Func<SystemConfiguration, bool>>? predicate, int page, int size);
        Task<Result<GetSystemConfigurationResponse>> Get(Guid id);
        Task<Result<bool>> Update(Guid id, UpdateSystemConfigurationRequest request);
        Task<SystemConfiguration> GetConfigByKey(string key);
    }
}
