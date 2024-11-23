using CatechistHelper.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.AbsenceRequest
{
    public class AbsenceApproveRequest
    {
        public Guid RequestId { get; set; }
        public Guid ApproverId { get; set; }
        public RequestStatus Status { get; set; }

        [MaxLength(100, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Comment { get; set; } = string.Empty;
    }
}
