using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("interview_process")]
    public class InterviewProcess : BaseEntity
    {
        [Column("name")]
        [StringLength(20)]
        public string Name { get; set; } = null!;

        [Column("status")]
        [EnumDataType(typeof(InterviewProcessStatus))]
        public InterviewProcessStatus InterviewProcessStatus { get; set; }

        [Column("application_id")]
        [ForeignKey(nameof(Application))]
        public Guid ApplicationId { get; set; }
        public virtual Application Application { get; set; } = null!;
    }
}
