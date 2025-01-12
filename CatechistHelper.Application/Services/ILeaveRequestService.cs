using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.LeaveRequest;
using CatechistHelper.Domain.Dtos.Responses.LeaveRequest;
using CatechistHelper.Domain.Entities;

namespace CatechistHelper.Application.Services
{
    public interface ILeaveRequestService
    {
        Task<Result<List<GetLeaveResponse>>> GetAll(LeaveRequestStatus? status, Guid? cId);
        Task<Result<bool>> ProcessLeaveRequest(LeaveApproveRequest absenceRequest);
        Task<Result<bool>> SubmitLeaveRequest(LeaveRequestDto leaveRequest);
    }
}
