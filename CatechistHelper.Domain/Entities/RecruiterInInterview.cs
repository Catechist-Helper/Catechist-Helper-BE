using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("recruiter_in_interview")]
    public class RecruiterInInterview
    {
        [Column("account_id")]
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;
        [Column("interview_id")]
        public Guid InterviewId { get; set; }
        public virtual Interview Interview { get; set; } = null!;
        [Column("online_room_url")]
        public string? OnlineRoomUrl { get; set; }
        [Column("evaluation")]
        public string? Evaluation { get; set; }
    }
}
