using CatechistHelper.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("absence_request")]
    public class AbsenceRequest : BaseEntity
    {
        [Column("catechist_id")]
        public Guid CatechistId { get; set; }

        [Column("slot_id")]
        public Guid SlotId { get; set; }

        [Column("reason")]
        [StringLength(100)]
        [Required]
        public string Reason { get; set; } = null!;

        [Column("status")]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        [Column("replacement_catechist_id")]
        public Guid? ReplacementCatechistId { get; set; }

        [Column("approver_id")]
        public Guid? ApproverId { get; set; }

        [Column("comment")]
        [StringLength(100)]
        public string? Comment { get; set; }

        [Column("approval_date")]
        public DateTime? ApprovalDate { get; set; }

        public virtual Slot Slot { get; set; } = null!;
        public virtual Catechist Catechist { get; set; } = null!;
        public virtual Catechist? ReplacementCatechist { get; set; }
        public virtual Account? Approver { get; set; }

        public ICollection<RequestImage>? RequestImages { get; set; } = [];
    }

    public enum RequestStatus : byte
    {
        Pending,
        Approved,
        Rejected
    }
}
