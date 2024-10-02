using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Room;
using CatechistHelper.Domain.Dtos.Responses.Room;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface IRoomService
    {
        Task<PagingResult<GetRoomResponse>> GetPagination(Expression<Func<Room, bool>>? predicate, int page, int size);
        Task<Result<GetRoomResponse>> Get(Guid id);
        Task<Result<GetRoomResponse>> Create(CreateRoomRequest request);
        Task<Result<bool>> Update(Guid id, UpdateRoomRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
