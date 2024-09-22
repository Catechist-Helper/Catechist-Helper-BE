using CatechistHelper.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("account")]
    public class Account : BaseEntity
    {
        [Column("email", TypeName = "varchar")]
        [StringLength(50)]
        [Required]
        public string Email { get; set; } = null!;

        [Column("hashed_password")]
        [StringLength(50)]
        [Required]
        public string HashedPassword { get; set; } = null!;

        [Column("fullname")]
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
        [Required]
        public string BirthPlace { get; set; } = null!;

        [Column("father_name")]
        [StringLength(50)]
        [Required]
        public string FatherName { get; set; } = null!;

        [Column("father_phone")]
        [StringLength(11)]
        [Required]
        public string FatherPhone { get; set; } = null!;

        [Column("mother_name")]
        [StringLength(50)]
        [Required]
        public string MotherName { get; set; } = null!;

        [Column("mother_phone")]
        [StringLength(11)]
        [Required]
        public string MotherPhone { get; set; } = null!;

        [Column("image_url")]
        public string ImageUrl { get; set; } = null!;

        [Column("address")]
        [StringLength(200)]
        public string Address { get; set; } = null!;

        [Column("phone")]
        [StringLength(11)]
        public string Phone { get; set; } = null!;

        [Column("role_id")]
        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;

        [InverseProperty(nameof(Account))]
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        public virtual Candidate? Candidate { get; set; }

        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
        public virtual ICollection<Recruiter> Recruiters { get; set; } = new List<Recruiter>();

        public virtual Catechist? Catechist { get; set; }

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Member> Members { get; set; } = new List<Member>();

        public virtual ICollection<Process> Processes { get; set; } = new List<Process>();
        public virtual ICollection<MemberOfProcess> MemberOfProcesses { get; set; } = new List<MemberOfProcess>();
    }
}
