using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("absent_request")]
    public class AbsenceRequest
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CatechistId { get; set; }
        public Guid SlotId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public Guid? ReplacementCatechistId { get; set; }

        public Guid? ApproverId { get; set; } 
        public string Comment { get; set; } = string.Empty; 
        public DateTime? ApprovalDate { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;

        public virtual Slot Slot { get; set; } = null!;
        public virtual Catechist Catechist { get; set; } = null!;
        public virtual Catechist? ReplacementCatechist { get; set; }
        public virtual Account? Approver { get; set; } 
    }

    public enum RequestStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
