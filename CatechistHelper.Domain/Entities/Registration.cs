﻿using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CatechistHelper.Domain.Common;

namespace CatechistHelper.Domain.Entities
{
    [Table("registration")]
    public class Registration : BaseEntity
    {
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

        [Column("address")]
        [StringLength(100)]
        [Required]
        public string Address { get; set; } = null!;

        [Column("email", TypeName = "varchar")]
        [StringLength(50)]
        [Required]
        public string Email { get; set; } = null!;

        [Column("phone")]
        [StringLength(11)]
        [Required]
        public string Phone { get; set; } = null!;

        [Column("is_teaching_before")]
        public bool IsTeachingBefore { get; set; }

        [Column("year_of_teaching")]
        public int YearOfTeaching { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        [InverseProperty(nameof(Registration))]
        public virtual ICollection<CertificateOfCandidate> CertificateOfCandidates { get; set; } = new List<CertificateOfCandidate>();

        [Column("status")]
        [EnumDataType(typeof(RegistrationStatus))]
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;


        [InverseProperty(nameof(Registration))]
        public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();

        [InverseProperty(nameof(Registration))]
        public virtual ICollection<InterviewProcess> InterviewProcesses { get; set; } = new List<InterviewProcess>();

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual ICollection<Recruiter> Recruiters { get; set; } = new List<Recruiter>();
    }
}