using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("major")]
    public class Major
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        [StringLength(20)]
        public string Name { get; set; } = null!;

        [Column("hierarchy_level")]
        public int HierarchyLevel { get; set; }

        public virtual ICollection<Level> Levels { get; set; } = new List<Level>();
        public virtual ICollection<TeachingQualification> TeachingQualifications { get; set; } = new List<TeachingQualification>();

        [InverseProperty(nameof(Major))]
        public ICollection<Grade> Grades { get; set; } = new List<Grade>(); 
    }
}
