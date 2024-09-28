using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("interview_process")]
    public class InterviewProcess
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        [StringLength(20)]
        public string Name { get; set; } = null!;

        [Column("status")]
        [EnumDataType(typeof(InterviewProcessStatus))]
        public InterviewProcessStatus InterviewProcessStatus { get; set; }

        [Column("registration_id")]
        [ForeignKey(nameof(Registration))]
        public Guid RegistrationId { get; set; }
        public virtual Registration Registration { get; set; } = null!;
    }
}
