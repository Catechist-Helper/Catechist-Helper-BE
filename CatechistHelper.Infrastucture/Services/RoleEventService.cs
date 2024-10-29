using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.RoleEvent;
using CatechistHelper.Domain.Dtos.Responses.RoleEvent;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class RoleEventService : BaseService<RoleEventService>, IRoleEventService
    {
        public RoleEventService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<RoleEventService> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetRoleEventResponse>> Create(CreateRoleEventRequest request)
        {
            RoleEvent roleEvent = request.Adapt<RoleEvent>();
            RoleEvent result = await _unitOfWork.GetRepository<RoleEvent>().InsertAsync(roleEvent);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception(MessageConstant.Event.Fail.Create);
            }
            return Success(result.Adapt<GetRoleEventResponse>());
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                RoleEvent roleEvent = await _unitOfWork.GetRepository<RoleEvent>().SingleOrDefaultAsync(
                    predicate: re => re.Id.Equals(id)) ?? throw new Exception(MessageConstant.RoleEvent.Fail.NotFound);
                _unitOfWork.GetRepository<RoleEvent>().DeleteAsync(roleEvent);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.RoleEvent.Fail.Delete);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<GetRoleEventResponse>> Get(Guid id)
        {
            RoleEvent roleEvent = await _unitOfWork.GetRepository<RoleEvent>().SingleOrDefaultAsync(
                predicate: e => e.Id == id) ?? throw new Exception(MessageConstant.RoleEvent.Fail.NotFound);
            return Success(roleEvent.Adapt<GetRoleEventResponse>());
        }

        public async Task<PagingResult<GetRoleEventResponse>> GetPagination(int page, int size)
        {
            IPaginate<RoleEvent> roleEvents =
                   await _unitOfWork.GetRepository<RoleEvent>().GetPagingListAsync(
                            page: page,
                            size: size);
            return SuccessWithPaging(
                    roleEvents.Adapt<IPaginate<GetRoleEventResponse>>(),
                    page,
                    size,
                    roleEvents.Total);
        }

        public async Task<Result<bool>> Update(Guid id, UpdateRoleEventRequest request)
        {
            try
            {
                RoleEvent roleEvent = await _unitOfWork.GetRepository<RoleEvent>().SingleOrDefaultAsync(
                    predicate: m => m.Id.Equals(id)) ?? throw new Exception(MessageConstant.RoleEvent.Fail.NotFound);
                request.Adapt(roleEvent);
                _unitOfWork.GetRepository<RoleEvent>().UpdateAsync(roleEvent);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.RoleEvent.Fail.Update);
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