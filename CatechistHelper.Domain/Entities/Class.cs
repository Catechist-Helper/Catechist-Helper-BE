using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("class")]
    public class Class : BaseEntity
    {
        [Column("name")]
        [StringLength(20)]
        public string Name { get; set; } = null!;

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("number_of_catechist")]
        public int NumberOfCatechist {  get; set; }

        [Column("note")]
        [StringLength(500)]
        public string? Note { get; set; }

        [Column("status")]
        [EnumDataType(typeof(ClassStatus))] 
        public ClassStatus ClassStatus { get; set; } = ClassStatus.Active;

        [Column("pastoral_year_id")]
        [ForeignKey(nameof(PastoralYear))]
        public Guid PastoralYearId { get; set; }
        public PastoralYear PastoralYear { get; set; } = null!;

        [Column("grade_id")]
        [ForeignKey(nameof(Grade))]
        public Guid GradeId { get; set; }
        public Grade Grade { get; set; } = null!;

        public virtual ICollection<Catechist> Catechists { get; set; } = new List<Catechist>();
        public virtual ICollection<CatechistInClass> CatechistInClasses { get; set; } = new List<CatechistInClass>();

        [InverseProperty(nameof(Class))]
        public ICollection<Slot> Slots { get; set; } = new List<Slot>();
    }
}
