using CatechistHelper.Domain.Dtos.Responses.Certificate;

namespace CatechistHelper.Domain.Dtos.Responses.CertificateOfCatechist
{
    public class GetCertificateOfCatechistResponse
    {
        public required GetCertificateResponse Certificate { get; set; }
        public DateTime GrantedDate { get; set; }
    }
}
