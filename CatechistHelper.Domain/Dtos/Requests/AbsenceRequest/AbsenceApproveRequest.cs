using CatechistHelper.Domain.Entities;

namespace CatechistHelper.Domain.Dtos.Requests.AbsenceRequest
{
    public class AbsenceApproveRequest
    {
        public Guid RequestId { get; set; }
        public Guid ApproverId { get; set; }
        public RequestStatus Status { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
