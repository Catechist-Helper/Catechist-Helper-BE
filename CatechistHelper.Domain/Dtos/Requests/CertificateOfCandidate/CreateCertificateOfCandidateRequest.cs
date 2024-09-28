using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.CertificateOfCandidate
{
    public class CreateCertificateOfCandidateRequest
    {
        [Required]
        public Guid RegistrationId { get; set; }
        [Required]
        public string ImageUrl { get; set; } = null!;
    }
}
