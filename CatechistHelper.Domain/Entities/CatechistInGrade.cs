using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("catechist_in_grade")]
    public class CatechistInGrade
    {
        [Column("grade_id")]
        [ForeignKey(nameof(Grade))]
        public Guid GradeId { get; set; }
        public Grade Grade { get; set; } = null!;

        [Column("catechist_id")]
        [ForeignKey(nameof(Catechist))]
        public Guid CatechisteId { get; set; }
        public Catechist Catechist { get; set; } = null!;

        [Column("is_main")]
        public bool IsMain { get; set; }

    }
}
