using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Entities
{
    [Table("receipt_image")]
    public class ReceiptImage
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("image_url")]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;  // URL or file path of the image

        [Column("created_at")]
        public DateTime UploadAt { get; set; } = DateTime.Now;  // Timestamp for when the image was uploaded

        [Column("process_id")]
        [ForeignKey(nameof(BudgetTransaction))]
        public Guid ProcessId { get; set; }  // Foreign key to Process
        public virtual Process Process { get; set; } = null!;
    }
}
