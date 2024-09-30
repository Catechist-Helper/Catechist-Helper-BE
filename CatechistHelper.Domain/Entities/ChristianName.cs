using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Entities
{
    [Table("christian_name")]
    public class ChristianName
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("gender")]
        [StringLength(10)]
        public string? Gender { get; set; }

        [Column("holy_day")]
        public DateTime? HolyDay { get; set; }

        [InverseProperty(nameof(ChristianName))]
        public virtual ICollection<Catechist> Catechists { get; set; } = new List<Catechist>();
    }
}
