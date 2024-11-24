using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("interview")]
    public class Interview
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("meeting_time")]
        public DateTime MeetingTime { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        [Column("is_passed")]
        public bool IsPassed { get; set; }

        [Column("registration_id")]
        [ForeignKey(nameof(Registration))]
        public Guid RegistrationId { get; set; }
        public Registration Registration { get; set; } = null!;

        [Column("interview_type")]
        [EnumDataType(typeof(InterviewType))]
        public InterviewType InterviewType { get; set; } = InterviewType.Offline;
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual ICollection<RecruiterInInterview> RecruiterInInterviews { get; set; } = new List<RecruiterInInterview>();
    }

    public enum InterviewType
    {
        Online,
        Offline
    }
}
