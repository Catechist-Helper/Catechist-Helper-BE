using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("pastoral_year")]
    public class PastoralYear
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("note")]
        [StringLength(500)]
        public string? Note { get; set; }

        [Column("status")]
        [EnumDataType(typeof(PastoralYearStatus))]
        public PastoralYearStatus PastoralYearStatus { get; set; }

        [InverseProperty(nameof(PastoralYear))]
        public ICollection<Class> Classes { get; set; } = new List<Class>();

        [InverseProperty(nameof(PastoralYear))]
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
