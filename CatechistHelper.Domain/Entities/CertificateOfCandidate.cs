using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("certificate_of_candidate")]
    public class CertificateOfCandidate
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("image_url", TypeName = "varchar")]
        [StringLength(500)]
        public string ImageUrl { get; set; } = null!;

        [Column("registration_id")]
        [ForeignKey(nameof(Registration))]
        public Guid RegistrationId { get; set; } 
        public Registration Registration { get; set; } = null!;
    }
}
