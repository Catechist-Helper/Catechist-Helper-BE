using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("grade")]
    public class Grade
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("major_id")]
        [ForeignKey(nameof(Major))]
        public Guid MajorId { get; set; }
        public Major Major { get; set; } = null!;

        [InverseProperty(nameof(Grade))]
        public ICollection<Class> Classes { get; set; } = new List<Class>();

        [Column("pastoral_year_id")]
        [ForeignKey(nameof(PastoralYear))]
        public Guid PastoralYearId { get; set; }
        public PastoralYear PastoralYear { get; set; } = null!;

        public virtual ICollection<Catechist> Catechists { get; set; } = new List<Catechist>();
        public virtual ICollection<CatechistInGrade> CatechistInGrades { get; set; } = new List<CatechistInGrade>();

    }
}
