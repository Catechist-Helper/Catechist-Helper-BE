using CatechistHelper.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("participant_in_event")]
    public class ParticipantInEvent : BaseEntity
    {
        [Column("full_name")]
        [StringLength(50)]
        public string FullName { get; set; } = null!;

        [Column("email", TypeName = "varchar")]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [Column("phone")]
        [StringLength(11)]
        public string Phone { get; set; } = null!;

        [Column("gender")]
        [StringLength(10)]
        public string Gender { get; set; } = null!;

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("address")]
        [StringLength(100)]
        public string Address { get; set; } = null!;

        [Column("is_attended")]
        public bool IsAttended { get; set; }

        [Column("event_id")]
        [ForeignKey(nameof(Event))]
        public Guid EventId { get; set; }
        public Event Event { get; set; } = null!;
    }
}
