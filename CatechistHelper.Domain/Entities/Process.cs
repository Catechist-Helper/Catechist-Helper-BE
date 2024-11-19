using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("process")]
    public class Process
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; } = null!;

        [Column("duration")]
        public TimeSpan Duration { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("fee")]
        public double Fee { get; set; }

        [Column("status")]
        [EnumDataType(typeof(ProcessStatus))]
        public ProcessStatus Status { get; set; }

        [Column("event_id")]
        [ForeignKey(nameof(Event))]
        public Guid EventId { get; set; }
        public Event Event { get; set; } = null!;

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual ICollection<MemberOfProcess> MemberOfProcesses { get; set; } = new List<MemberOfProcess>();

    }
}
