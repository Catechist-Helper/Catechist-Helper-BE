namespace CatechistHelper.Domain.Dtos.Responses.Certificate
{
    public class GetCertificateResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public Guid LevelId { get; set; }
    }
}
