using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Entities
{
    [Table("transaction_image")]
    public class TransactionImage
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("budget_transaction_id")]
        [ForeignKey(nameof(BudgetTransaction))]
        public Guid BudgetTransactionId { get; set; }  // Foreign key to BudgetTransaction
        public virtual BudgetTransaction BudgetTransaction { get; set; } = null!;

        [Column("image_url")]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;  // URL or file path of the image

        [Column("created_at")]
        public DateTime UploadAt { get; set; } = DateTime.Now;  // Timestamp for when the image was uploaded
    }

}
