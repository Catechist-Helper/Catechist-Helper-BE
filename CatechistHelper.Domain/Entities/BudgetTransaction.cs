using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("budget_transaction")]
    public class BudgetTransaction
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("from_budget")]
        public double FromBudget { get; set; }

        [Column("to_budget")]
        public double ToBudget { get; set; }

        [Column("transaction_at")]
        public DateTime TransactionAt { get; set; } = DateTime.Now;

        [Column("note")]
        [StringLength(500)]
        public string? Note { get; set; }

        [Column("event_id")]
        [ForeignKey(nameof(Event))]
        public Guid EventId { get; set; }
        public Event Event { get; set; } = null!;

        public ICollection<TransactionImage> TransactionImages { get; set; } = [];
    }
}
