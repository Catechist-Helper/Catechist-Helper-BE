using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.RoleEvent;
using CatechistHelper.Domain.Dtos.Responses.RoleEvent;

namespace CatechistHelper.Application.Services
{
    public interface IRoleEventService
    {
        Task<PagingResult<GetRoleEventResponse>> GetPagination(int page, int size);
        Task<Result<GetRoleEventResponse>> Get(Guid id);
        Task<Result<GetRoleEventResponse>> Create(CreateRoleEventRequest request);
        Task<Result<bool>> Update(Guid id, UpdateRoleEventRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
