using CatechistHelper.Domain.Dtos.Requests.AbsenceRequest;
using CatechistHelper.Domain.Entities;
namespace CatechistHelper.Domain.Dtos.Responses.AbsenceRequest
{
    public class GetAbsentRequest : AbsenceRequestDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public Guid? ApproverId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime? ApprovalDate { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public virtual Entities.Slot Slot { get; set; } = null!;
        public string CatechistName { get; set; } = string.Empty;
        public string? ReplacementCatechistName { get; set; }
        public string? Approver { get; set; }
    }
}
