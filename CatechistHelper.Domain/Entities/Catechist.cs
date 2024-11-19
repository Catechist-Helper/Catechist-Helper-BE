using CatechistHelper.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("catechist")]
    public class Catechist : BaseEntity
    {
        [Column("code")]
        public string Code { get; set; } = null!;

        [Column("full_name")]
        [StringLength(50)]
        [Required]
        public string FullName { get; set; } = null!;

        [Column("gender")]
        [StringLength(10)]
        [Required]
        public string Gender { get; set; } = null!;

        [Column("date_of_birth")]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Column("birth_place")]
        [StringLength(20)]
        public string? BirthPlace { get; set; }

        [Column("father_name")]
        [StringLength(50)]
        public string? FatherName { get; set; }

        [Column("father_phone", TypeName = "varchar")]
        [StringLength(10)]
        public string? FatherPhone { get; set; }

        [Column("mother_name")]
        [StringLength(50)]
        public string? MotherName { get; set; }

        [Column("mother_phone", TypeName = "varchar")]
        [StringLength(10)]
        public string? MotherPhone { get; set; }

        [Column("image_url", TypeName = "varchar")]
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Column("address")]
        [StringLength(200)]
        public string? Address { get; set; }

        [Column("phone", TypeName = "varchar")]
        [StringLength(10)]
        public string? Phone { get; set; }

        [Column("qualification")]
        [StringLength(50)]
        public string? Qualification { get; set; }

        [Column("is_teaching")]
        public bool IsTeaching { get; set; } = true;

        [Column("note")]
        public string? Note { get; set; }

        [Column("account_id")] 
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

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

        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
        public virtual ICollection<CatechistInGrade> CatechistInGrades { get; set; } = new List<CatechistInGrade>();

        public virtual ICollection<TrainingList> TrainingLists { get; set; } = new List<TrainingList>();
        public virtual ICollection<CatechistInTraining> CatechistInTrainings { get; set; } = new List<CatechistInTraining>();

        public virtual ICollection<AbsenceRequest> AbsenceRequests { get; set; } = new List<AbsenceRequest>();
        public virtual ICollection<AbsenceRequest> ReplacementAbsenceRequests { get; set; } = new List<AbsenceRequest>();
    }
}
