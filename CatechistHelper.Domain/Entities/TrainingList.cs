using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("training_list")]
    public class TrainingList : BaseEntity
    {
        [Column("previous_level")]
        [StringLength(20)]
        public string PreviousLevel { get; set; } = null!;

        [Column("next_level")]
        [StringLength(20)]
        public string NextLevel { get; set; } = null!;

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("status")]
        [EnumDataType(typeof(TrainingListStatus))]
        public TrainingListStatus TrainingListStatus { get; set; } = TrainingListStatus.Training;

        public virtual ICollection<Catechist> Catechists { get; set; } = new List<Catechist>();
        public virtual ICollection<CatechistInTraining> CatechistInTrainings { get; set; } = new List<CatechistInTraining>();
    }
}
