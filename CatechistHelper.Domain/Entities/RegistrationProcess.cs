using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("registration_process")]
    public class RegistrationProcess
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        [StringLength(20)]
        public string Name { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("status")]
        [EnumDataType(typeof(RegistrationProcessStatus))]
        public RegistrationProcessStatus Status { get; set; }

        [Column("registration_id")]
        [ForeignKey(nameof(Registration))]
        public Guid RegistrationId { get; set; }
        public virtual Registration Registration { get; set; } = null!;
    }
}
