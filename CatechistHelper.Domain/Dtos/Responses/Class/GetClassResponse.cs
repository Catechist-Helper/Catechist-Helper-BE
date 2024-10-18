using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Responses.Class
{
    public class GetClassResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int NumberOfCatechist { get; set; }
        public string? Note { get; set; }
        public ClassStatus ClassStatus { get; set; }
        public string PastoralYearName { get; set; } = string.Empty;
        public string GradeName { get; set; } = string.Empty;
    }
}
