using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("member")]
    public class Member
    {
        [Column("account_id")]
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;

        [Column("event_id")]
        public Guid EventId { get; set; }
        public virtual Event Event { get; set; } = null!;

        [Column("role_event_id")]
        [ForeignKey(nameof(RoleEvent))]
        public Guid RoleEventId { get; set; }
        public RoleEvent RoleEvent { get; set; } = null!;
    }
}
