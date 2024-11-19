using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("event")]
    public class Event : BaseEntity
    {
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("description")]
        [StringLength(1000)]
        public string Description { get; set; } = null!;

        [Column("is_periodic")]
        public bool IsPeriodic { get; set; }

        [Column("is_checked_in")]
        public bool IsCheckedIn { get; set; }

        [Column("address")]
        [StringLength(200)]
        public string? Address { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("current_budget")]
        public double current_budget { get; set; } = 0;

        [Column("status")]
        [EnumDataType(typeof(EventStatus))]
        public EventStatus EventStatus { get; set; }

        [Column("event_category_id")]
        [ForeignKey(nameof(EventCategory))]
        public Guid EventCategoryId { get; set; }
        public virtual EventCategory EventCategory { get; set; } = null!;

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual ICollection<Member> Members { get; set; } = new List<Member>();

        [InverseProperty(nameof(Event))] 
        public virtual ICollection<BudgetTransaction> BudgetTransactions { get; set; } = new List<BudgetTransaction>();

        [InverseProperty(nameof(Event))]
        public virtual ICollection<Process> Processes { get; set; } = new List<Process>();

        [InverseProperty(nameof(Event))]
        public virtual ICollection<ParticipantInEvent> ParticipantInEvents { get; set; } = new List<ParticipantInEvent>();
    }
}
