﻿using CatechistHelper.Domain.Common;
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

        [Column("hashed_password", TypeName = "varchar")]
        [StringLength(50)]
        [Required]
        public string HashedPassword { get; set; } = null!;

        [Column("full_name")]
        [StringLength(50)]
        [Required]
        public string FullName { get; set; } = null!;

        [Column("gender")]
        [StringLength(10)]
        public string? Gender { get; set; }

        [Column("phone", TypeName = "varchar")]
        [StringLength(10)]
        public string Phone { get; set; } = null!;

        [Column("avatar", TypeName = "varchar")]
        [StringLength(500)]
        public string? Avatar { get; set; }

        [Column("role_id")]
        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;

        [InverseProperty(nameof(Account))]
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        public virtual Catechist? Catechist { get; set; }

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Member> Members { get; set; } = new List<Member>();

        public virtual ICollection<Process> Processes { get; set; } = new List<Process>();
        public virtual ICollection<MemberOfProcess> MemberOfProcesses { get; set; } = new List<MemberOfProcess>();

        public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();
        public virtual ICollection<RecruiterInInterview> RecruiterInInterviews { get; set; } = new List<RecruiterInInterview>();

        public virtual ICollection<AbsenceRequest> AbsenceRequests { get; set; } = new List<AbsenceRequest>();
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    }
}
