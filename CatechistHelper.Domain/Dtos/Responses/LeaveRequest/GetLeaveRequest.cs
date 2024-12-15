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

    public class GetLeaveResponse
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string CatechistId { get; set; }
        public string CatechistName { get; set; }
        public string? Approver { get; set; }
        public RequestStatus Status { get; set; }
        public Guid? ApproverId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
