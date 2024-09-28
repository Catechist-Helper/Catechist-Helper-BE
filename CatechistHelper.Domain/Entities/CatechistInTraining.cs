using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("catechist_in_training")]
    public class CatechistInTraining
    {
        [Column("training_list_id")]
        public Guid TrainingListId { get; set; }
        public virtual TrainingList TrainingList { get; set; } = null!;

        [Column("catechist_id")]
        public Guid CatechistId { get; set; }
        public virtual Catechist Catechist { get; set; } = null!;

        [Column("status")]
        public CatechistInTrainingStatus CatechistInTrainingStatus { get; set; }
    }
}
