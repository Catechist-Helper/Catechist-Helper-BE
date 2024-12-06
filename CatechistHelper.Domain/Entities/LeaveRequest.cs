using CatechistHelper.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Entities
{
    [Table("absence_request")]
    public class LeaveRequest : BaseEntity
    {
        [Column("catechist_id")]
        public Guid CatechistId { get; set; }

        [Column("leave_date")]
        public DateTime LeaveDate { get; set; }

        [Column("reason")]
        [StringLength(100)]
        [Required]
        public string Reason { get; set; } = null!;

        [Column("status")]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        [Column("approver_id")]
        public Guid? ApproverId { get; set; }

        [Column("comment")]
        [StringLength(100)]
        public string? Comment { get; set; }

        [Column("approval_date")]
        public DateTime? ApprovalDate { get; set; }

        public virtual Slot Slot { get; set; } = null!;
        public virtual Catechist Catechist { get; set; } = null!;
        public virtual Account? Approver { get; set; }
    }
}
