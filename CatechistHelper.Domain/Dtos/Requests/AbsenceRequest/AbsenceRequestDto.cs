using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.AbsenceRequest
{
    public class AbsenceRequestDto
    {
        public Guid CatechistId { get; set; }
        public Guid SlotId { get; set; }

        [MaxLength(100, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Reason { get; set; } = string.Empty;
        public Guid? ReplacementCatechistId { get; set; }
    }
}
