using CatechistHelper.Domain.Dtos.Requests.LeaveRequest;
using CatechistHelper.Domain.Entities;

namespace CatechistHelper.Domain.Dtos.Responses.LeaveRequest
{
    public class GetLeaveRequest : LeaveRequestDto
    {
        public string Comment { get; set; } = string.Empty;
        public DateTime? ApprovalDate { get; set; }
        public string CatechistName { get; set; } = string.Empty;
        public string? Approver { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public Guid? ApproverId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public class GetLeaveResponse : GetLeaveRequest
    {
        public Guid Id { get; set; }
    }
}
