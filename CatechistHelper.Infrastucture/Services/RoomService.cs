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
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<PagingResult<GetRoomResponse>> GetPagination(Guid? pastoralYearId, Guid? slotId, int page, int size, bool excludeRoomAssigned = false)
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
                            predicate: s => s.Class.PastoralYearId == pastoralYearId
                            && s.RoomId.HasValue
                            && s.Date > DateTime.Now,
                            selector: s => s.RoomId.Value);
                }
                IPaginate<Room> rooms =
                    await _unitOfWork.GetRepository<Room>()
                    .GetPagingListAsync(
                            predicate: r => r.IsDeleted == false
                                            && (!excludeRoomAssigned || !assignedRoomIds.Contains(r.Id)),
                            page: page,
                            size: size
                        );
                
                if(pastoralYearId != null && slotId != null)
                {
                    PastoralYear SelectedPastoralYear = await _unitOfWork.GetRepository<PastoralYear>().SingleOrDefaultAsync(
                        predicate: py => py.Id == pastoralYearId) ?? throw new Exception(MessageConstant.PastoralYear.Fail.NotFoundPastoralYear);
                    Slot selectedSlot= await _unitOfWork.GetRepository<Slot>().SingleOrDefaultAsync(
                                            predicate: py => py.Id == slotId) ?? throw new Exception();

                    // Kết hợp ngày từ date và giờ từ startTime
                    DateTime combinedDateStartTime = new DateTime(
                        selectedSlot.Date.Year,
                        selectedSlot.Date.Month,
                        selectedSlot.Date.Day,
                        selectedSlot.StartTime.Hour,
                        selectedSlot.StartTime.Minute,
                        selectedSlot.StartTime.Second,
                        DateTimeKind.Utc
                    );

                    DateTime combinedDateEndTime = new DateTime(
                        selectedSlot.Date.Year,
                        selectedSlot.Date.Month,
                        selectedSlot.Date.Day,
                        selectedSlot.EndTime.Hour,
                        selectedSlot.EndTime.Minute,
                        selectedSlot.EndTime.Second,
                        DateTimeKind.Utc
                    );

                    // Lấy tất cả các slot cần thiết và chuyển về client để đánh giá
                    var allSlots = await _unitOfWork.GetRepository<Slot>()
                        .GetListAsync(
                            predicate: s => s.Class.PastoralYearId == pastoralYearId,
                            selector: s => new {
                                s.RoomId,
                                s.Date,
                                s.StartTime,
                                s.EndTime
                            });

                    // Kiểm tra giao thoa thời gian trên client side
                    var selectedAssignedRoomIds = allSlots
                        .Where(s => combinedDateStartTime <= new DateTime(
                                        s.Date.Year,
                                        s.Date.Month,
                                        s.Date.Day,
                                        s.EndTime.Hour,
                                        s.EndTime.Minute,
                                        s.EndTime.Second,
                                        DateTimeKind.Utc
                                    )
                                    && combinedDateEndTime >= new DateTime(
                                        s.Date.Year,
                                        s.Date.Month,
                                        s.Date.Day,
                                        s.StartTime.Hour,
                                        s.StartTime.Minute,
                                        s.StartTime.Second,
                                        DateTimeKind.Utc
                                    ))
                        .Select(s => s.RoomId ?? Guid.Empty)
                        .ToList();


                    IPaginate<Room> selectedRooms =
                    await _unitOfWork.GetRepository<Room>()
                    .GetPagingListAsync(
                            predicate: r => r.IsDeleted == false
                                            && !selectedAssignedRoomIds.Contains(r.Id),
                            page: page,
                            size: size
                        );

                    return SuccessWithPaging(
                        selectedRooms.Adapt<IPaginate<GetRoomResponse>>(),
                        page,
                        size,
                        selectedRooms.Total);
                }

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

                if (request.Image != null)
                {
                    if (room.Image != null)
                    {
                        await _firebaseService.DeleteImageAsync(room.Image);
                        room.Image = null;
                    }
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
