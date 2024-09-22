using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CatechistHelper.Domain.Common;

namespace CatechistHelper.Domain.Entities
{
    [Table("candidate")]
    public class Candidate : BaseEntity
    {
        [Column("first_name", TypeName = "varchar")]
        [StringLength(20)]
        [Required]
        public string FirstName { get; set; } = null!;

        [Column("last_name")]
        [StringLength(20)]
        [Required]
        public string LastName { get; set; } = null!;

        [Column("date_of_birth")]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Column("address")]
        [StringLength(100)]
        public string Address { get; set; } = null!;

        [Column("city")]
        [StringLength(20)]
        public string City { get; set; } = null!;

        [Column("email", TypeName = "varchar")]
        [StringLength(50)]
        [Required]
        public string Email { get; set; } = null!;

        [Column("phone")]
        [StringLength(11)]
        public string Phone { get; set; } = null!;

        [Column("is_teaching_before")]
        public bool IsTeachingBefore { get; set; }

        [Column("year_of_teaching")]
        public int YearOfTeaching { get; set; }

        [Column("note")]
        public string Note { get; set; } = null!;

        [Column("account_id")]
        [ForeignKey(nameof(Account))]
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;

        [InverseProperty(nameof(Candidate))]
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

        [InverseProperty(nameof(Candidate))]
        public virtual ICollection<CertificateOfCandidate> CertificateOfCandidates { get; set; } = new List<CertificateOfCandidate>();
    }
}
