using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("slot")]
    public class Slot
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("note")]
        [StringLength(500)]
        public string? Note { get; set; }

        [Column("class_id")]
        [ForeignKey(nameof(Class))]
        public Guid ClassId { get; set; }
        public Class Class { get; set; } = null!;

        [Column("room_id")]
        [ForeignKey(nameof(Room))]
        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public virtual ICollection<Catechist> Catechists { get; set; } = new List<Catechist>();
        public virtual ICollection<CatechistInSlot> CatechistInSlots { get; set; } = new List<CatechistInSlot>();
        public virtual ICollection<AbsenceRequest> AbsenceRequests { get; set; } = new List<AbsenceRequest>();
    }
}
