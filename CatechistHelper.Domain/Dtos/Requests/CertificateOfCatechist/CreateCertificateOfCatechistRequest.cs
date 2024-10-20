namespace CatechistHelper.Domain.Dtos.Requests.CertificateOfCatechist
{
    public class CreateCertificateOfCatechistRequest
    {
        public Guid CatechistId { get; set; }
        public Guid CertificateId { get; set; }
        public DateTime GrantedDate { get; set; }
    }
}