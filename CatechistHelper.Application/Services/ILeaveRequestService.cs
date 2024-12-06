using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.AbsenceRequest;
using CatechistHelper.Domain.Dtos.Requests.LeaveRequest;
using CatechistHelper.Domain.Dtos.Responses.AbsenceRequest;
using CatechistHelper.Domain.Dtos.Responses.LeaveRequest;
using CatechistHelper.Domain.Entities;

namespace CatechistHelper.Application.Services
{
    public interface ILeaveRequestService
    {
        Task<Result<List<GetLeaveRequest>>> GetAll(RequestStatus status, Guid? cId);
        Task<Result<bool>> ProcessLeaveRequest(AbsenceApproveRequest absenceRequest);
        Task<Result<bool>> SubmitLeaveRequest(LeaveRequestDto leaveRequest);
    }
}
