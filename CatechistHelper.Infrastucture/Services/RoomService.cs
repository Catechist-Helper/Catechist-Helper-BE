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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<PagingResult<GetRoomResponse>> GetPagination(Guid? pastoralYearId, int page, int size, bool excludeRoomAssigned = false)
        {
            try
            {
                if (pastoralYearId != null)
                {
                    PastoralYear pastoralYear = await _unitOfWork.GetRepository<PastoralYear>().SingleOrDefaultAsync(
                        predicate: py => py.Id == pastoralYearId) ?? throw new Exception(MessageConstant.PastoralYear.Fail.NotFoundPastoralYear);
                }
                ICollection<Guid> assignedRoomIds = new List<Guid>();
                if (excludeRoomAssigned && pastoralYearId != null)
                {
                    assignedRoomIds = await _unitOfWork.GetRepository<Slot>()
                        .GetListAsync(
                            predicate: s => s.Class.PastoralYearId == pastoralYearId,
                            selector: s => s.RoomId);
                }
                IPaginate<Room> rooms =
                    await _unitOfWork.GetRepository<Room>()
                    .GetPagingListAsync(
                            predicate: r => r.IsDeleted == false
                                            && (!excludeRoomAssigned || !assignedRoomIds.Contains(r.Id)),
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
                throw;
            }
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
