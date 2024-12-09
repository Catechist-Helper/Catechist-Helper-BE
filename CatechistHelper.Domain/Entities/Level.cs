﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("level")]
    public class Level
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        [StringLength(10)]
        public string Name { get; set; } = null!;

        [Column("description")]
        [StringLength(100)]
        public string? Description { get; set; }

        [Column("hierarchy_level")]
        public int HierarchyLevel { get; set; }

        [InverseProperty(nameof(Level))]
        public virtual ICollection<Catechist> Catechists { get; set; } = new List<Catechist>();

        [InverseProperty(nameof(Level))]
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

        public virtual ICollection<Major> Majors { get; set; } = new List<Major>();
        public virtual ICollection<TeachingQualification> TeachingQualifications { get; set; } = new List<TeachingQualification>();

        public virtual ICollection<TrainingList> NextLevelTrainings { get; set; } = new List<TrainingList>();
        public virtual ICollection<TrainingList> PreviousLevelTrainings { get; set; } = new List<TrainingList>();
    }
}
