using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("certificate")]
    public class Certificate
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("description")]
        [StringLength(100)]
        public string? Description { get; set; }

        [Column("image")]
        public string? Image {  get; set; }

        [Column("level_id")]
        [ForeignKey(nameof(Certificate))]
        public Guid LevelId { get; set; }
        public virtual Level Level { get; set; } = null!;

        public virtual ICollection<Catechist> Catechists { get; set; } = new List<Catechist>();
        public virtual ICollection<CertificateOfCatechist> CertificateOfCatechists { get; set; } = new List<CertificateOfCatechist>();
    }
}
