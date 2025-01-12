using CatechistHelper.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.LeaveRequest
{
    public class LeaveApproveRequest
    {
        public Guid RequestId { get; set; }
        public Guid ApproverId { get; set; }
        public LeaveRequestStatus Status { get; set; }

        [MaxLength(100, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Comment { get; set; } = string.Empty;
    }
}
