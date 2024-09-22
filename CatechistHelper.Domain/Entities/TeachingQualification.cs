using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("teaching_qualification")]
    public class TeachingQualification
    {
        [Column("major_id")]
        public Guid MajorId { get; set; }
        public virtual Major Major { get; set; } = null!;

        [Column("level_id")]
        public Guid LevelId { get; set; }
        public virtual Level Level { get; set; } = null!;
    }
}
