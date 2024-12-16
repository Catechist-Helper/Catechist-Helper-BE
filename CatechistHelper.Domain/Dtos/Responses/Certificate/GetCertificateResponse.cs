using CatechistHelper.Domain.Dtos.Responses.Level;

namespace CatechistHelper.Domain.Dtos.Responses.Certificate
{
    public class GetCertificateResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public GetLevelResponse? Level { get; set; }
    }
}
