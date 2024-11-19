using CatechistHelper.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("room")]
    public class Room : BaseEntity
    {
        [Column("name")]
        [StringLength(10)]
        public string Name { get; set; } = null!;

        [Column("description")]
        [StringLength(50)]
        public string? Description { get; set; }

        [Column("image_url", TypeName = "varchar")]
        [StringLength(500)]
        public string? Image { get; set; }

        [InverseProperty(nameof(Room))]
        public ICollection<Slot> Slots { get; set; } = new List<Slot>();

    }
}
