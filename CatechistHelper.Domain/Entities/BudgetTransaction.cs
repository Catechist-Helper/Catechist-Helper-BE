using CatechistHelper.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatechistHelper.Domain.Entities
{
    [Table("budget_transaction")]
    public class BudgetTransaction : BaseEntity
    {
        [Column("from_budget")]
        public double FromBudget { get; set; }

        [Column("to_budget")]
        public double ToBudget { get; set; }

        [Column("event_id")]
        [ForeignKey(nameof(Event))]
        public Guid EventId { get; set; }
        public Event Event { get; set; } = null!;
    }
}
