using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Entities
{
    [Table("event_category")]
    public class EventCategory
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [InverseProperty(nameof(EventCategory))]
        public ICollection<Event> Events { get; } = new List<Event>();
    }
}
