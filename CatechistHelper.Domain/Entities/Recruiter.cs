using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("recruiter")]
    public class Recruiter
    {
        [Column("account_id")]
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;

        [Column("registration_id")]
        public Guid RegistrationId { get; set; }
        public virtual Registration Registration { get; set; } = null!;
        public string? RoomUrl { get; set; }
    }
}
