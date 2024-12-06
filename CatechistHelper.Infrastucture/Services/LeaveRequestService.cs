using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.AbsenceRequest;
using CatechistHelper.Domain.Dtos.Requests.LeaveRequest;
using CatechistHelper.Domain.Dtos.Responses.LeaveRequest;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Utils;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class LeaveRequestService : BaseService<LeaveRequestService>, ILeaveRequestService
    {
        public LeaveRequestService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<LeaveRequestService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<List<GetLeaveRequest>>> GetAll(RequestStatus status, Guid? cId)
        {
            try
            {
                var absenceRequests = await _unitOfWork.GetRepository<LeaveRequest>()
                                   .GetListAsync(
                                       predicate: a => a.Status == status,
                                       include: a => a.Include(a => a.Catechist));
                if (cId != null)
                {
                    absenceRequests = absenceRequests.Where(a => a.Catechist.Id == cId).ToList();
                }

                var result = absenceRequests.Adapt<List<GetLeaveRequest>>();

                return Success(result);
            }
            catch (Exception ex)
            {
                return Fail<List<GetLeaveRequest>>(ex.Message);
            }
        }

        public async Task<Result<bool>> ProcessLeaveRequest(AbsenceApproveRequest absenceRequest)
        {
            try
            {
                var leaveRequest = await _unitOfWork.GetRepository<LeaveRequest>()
                    .SingleOrDefaultAsync(predicate: c => c.Id == absenceRequest.RequestId);

                Validator.EnsureNonNull(leaveRequest);

                if (absenceRequest.Status == RequestStatus.Approved)
                {
                    var catechistInSlots = await _unitOfWork.GetRepository<CatechistInSlot>()
                        .GetListAsync(predicate: c => c.CatechistId == leaveRequest.CatechistId && c.Slot.Date >= DateTime.Now);

                    if (catechistInSlots.Count != 0)
                    {
                        throw new Exception("Giao ly vien van con slot");
                    }
                }

                leaveRequest.Comment = absenceRequest.Comment;
                leaveRequest.ApproverId = absenceRequest.ApproverId;

                _unitOfWork.GetRepository<LeaveRequest>().UpdateAsync(leaveRequest);

                var catechist = await _unitOfWork.GetRepository<Catechist>()
                    .SingleOrDefaultAsync(predicate: c => c.Id == leaveRequest.CatechistId);
                Validator.EnsureNonNull(catechist);
                catechist.IsTeaching = false;

                return Success(await _unitOfWork.CommitAsync() > 0); 
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message); 
            }
        }


        public async Task<Result<bool>> SubmitLeaveRequest(LeaveRequestDto leaveRequest)
        {
            try
            {
                var absenceRequest = leaveRequest.Adapt<LeaveRequest>();

                await _unitOfWork.GetRepository<LeaveRequest>().InsertAsync(absenceRequest);

                return Success(await _unitOfWork.CommitAsync() > 0);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }
    }
}
