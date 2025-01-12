using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Dtos.Responses.Timetable;
using CatechistHelper.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CatechistHelper.Domain.Dtos.Responses.AbsenceRequest
{
    public class RequestImageResponse
    {
        public Guid Id { get; set; }
        public Guid AbsenceRequestId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UploadAt { get; set; }
    }
    public class GetAbsentRequest
    {
        public Guid CatechistId { get; set; }
        public Guid SlotId { get; set; }

        [MaxLength(100, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Reason { get; set; } = string.Empty;
        public Guid? ReplacementCatechistId { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public Guid? ApproverId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime? ApprovalDate { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string CatechistName { get; set; } = string.Empty;
        public string? ReplacementCatechistName { get; set; }
        public string? Approver { get; set; }
        public SlotResponse? Slot { get; set; }
        public string? PastoralYearName { get; set; }
        //public GetClassResponse? Class { get; set; }
        public List<RequestImageResponse>? RequestImages { get; set; }
    }
}
