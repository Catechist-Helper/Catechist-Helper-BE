using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("catechist_in_slot")]
    public class CatechistInSlot
    {
        [Column("slot_id")]
        public Guid SlotId { get; set; }
        public virtual Slot Slot { get; set; } = null!;

        [Column("catechist_id")]
        public Guid CatechistId { get; set; }
        public virtual Catechist Catechist { get; set; } = null!;

        [Column("type")]
        [StringLength(20)]
        public string Type { get; set; } = null!;
    }
}
