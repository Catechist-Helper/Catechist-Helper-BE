using CatechistHelper.Application.GoogleServices;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Room;
using CatechistHelper.Domain.Dtos.Responses.Room;
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
    public class RoomService : BaseService<RoomService>, IRoomService
    {
        private readonly IFirebaseService _firebaseService;
        public RoomService(IFirebaseService firebaseService, IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<RoomService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _firebaseService = firebaseService;
        }

        public async Task<Result<GetRoomResponse>> Create(CreateRoomRequest request)
        {
            try
            {
                Room roomFromDb = await _unitOfWork.GetRepository<Room>().SingleOrDefaultAsync(
                    predicate: r => r.Name.Equals(request.Name));

                if (roomFromDb != null)
                {
                    throw new Exception(MessageConstant.Room.Fail.NameAlreadyUsed);
                }

                Room room = request.Adapt<Room>();

                if (request.Image != null)
                {
                    string image = await _firebaseService.UploadImageAsync(request.Image, $"room/");
                    room.Image = image;
                }

                Room result = await _unitOfWork.GetRepository<Room>().InsertAsync(room);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Room.Fail.CreateRoom);
                }
                return Success(_mapper.Map<GetRoomResponse>(result));
            }
            catch (Exception ex)
            {
                return Fail<GetRoomResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                Room room = await _unitOfWork.GetRepository<Room>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                _unitOfWork.GetRepository<Room>().DeleteAsync(room);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Room.Fail.DeleteRoom);
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

        public async Task<Result<GetRoomResponse>> Get(Guid id)
        {
            try
            {
                Room room = await _unitOfWork.GetRepository<Room>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                return Success(room.Adapt<GetRoomResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetRoomResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetRoomResponse>> GetPagination(Expression<Func<Room, bool>>? predicate, int page, int size)
        {
            try
            {

                IPaginate<Room> rooms =
                    await _unitOfWork.GetRepository<Room>()
                    .GetPagingListAsync(
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        rooms.Adapt<IPaginate<GetRoomResponse>>(),
                        page,
                        size,
                        rooms.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<Result<bool>> Update(Guid id, UpdateRoomRequest request)
        {
            try
            {
                Room room = await _unitOfWork.GetRepository<Room>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                request.Adapt(room);

                if (room.Image != null)
                {
                    await _firebaseService.DeleteImageAsync(room.Image);
                    room.Image = null;
                }

                if (request.Image != null)
                {
                    string image = await _firebaseService.UploadImageAsync(request.Image, "room/");
                    room.Image = image;
                }

                _unitOfWork.GetRepository<Room>().UpdateAsync(room);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Room.Fail.UpdateRoom);
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
