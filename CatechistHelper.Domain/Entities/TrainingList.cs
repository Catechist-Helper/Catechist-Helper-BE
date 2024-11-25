using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("training_list")]
    public class TrainingList : BaseEntity
    {
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; } = null!;

        [Column("previous_level_id")]
        public Guid PreviousLevelId { get; set; }
        public Level PreviousLevel { get; set; } = null!;

        [Column("next_level_id")]
        public Guid NextLevelId { get; set; }
        public Level NextLevel { get; set; } = null!;

        [Column("certificate_id")]
        public Guid CertificateId { get; set; }
        public Certificate Certificate { get; set; } = null!;

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("status")]
        [EnumDataType(typeof(TrainingListStatus))]
        public TrainingListStatus TrainingListStatus { get; set; } = TrainingListStatus.NotStarted;

        public virtual ICollection<Catechist> Catechists { get; set; } = new List<Catechist>();
        public virtual ICollection<CatechistInTraining> CatechistInTrainings { get; set; } = new List<CatechistInTraining>();
    }
}
