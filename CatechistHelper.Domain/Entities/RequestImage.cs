using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("request_image")]
    public class RequestImage
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("request_id")]
        [ForeignKey(nameof(AbsenceRequest))]
        public Guid AbsenceRequestId { get; set; }  
        public virtual AbsenceRequest AbsenceRequest { get; set; } = null!;

        [Column("image_url")]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty; 

        [Column("created_at")]
        public DateTime UploadAt { get; set; } = DateTime.Now; 
    }
}
