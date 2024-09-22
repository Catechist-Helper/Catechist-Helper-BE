using CatechistHelper.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("catechist")]
    public class Catechist : BaseEntity
    {
        [Column("code")]
        public string Code { get; set; } = null!;

        [Column("qualification")]
        public string Qualification { get; set; } = null!;

        [Column("is_teaching")]
        public bool IsTeaching { get; set; }

        [Column("note")]
        public string Note { get; set; } = null!;

        [Column("account_id")] 
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        [InverseProperty(nameof(Catechist))]
        public virtual ICollection<TrainingList> TrainingLists { get; set; } = new List<TrainingList>();

        [Column("christian_name_id")]
        [ForeignKey(nameof(ChristianName))]
        public Guid ChristianNameId { get; set; }
        public virtual ChristianName? ChristianName { get; set; }

        [Column("level_id")]
        [ForeignKey(nameof(Level))]
        public Guid LevelId { get; set; }
        public virtual Level Level { get; set; } = null!;

        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public virtual ICollection<CertificateOfCatechist> CertificateOfCatechists { get; set; } = new List<CertificateOfCatechist>();

        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        public virtual ICollection<CatechistInClass> CatechistInClasses { get; set; } = new List<CatechistInClass>();

        public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
        public virtual ICollection<CatechistInSlot> CatechistInSlots { get; set; } = new List<CatechistInSlot>();
    }
}
