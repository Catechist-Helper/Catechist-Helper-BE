using CatechistHelper.Domain.Dtos.Responses.Major;
using CatechistHelper.Domain.Dtos.Responses.PastoralYear;

namespace CatechistHelper.Domain.Dtos.Responses.Grade
{
    public class GetGradeResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public required GetMajorResponse Major { get; set; } 
        public required GetPastoralYearResponse PastoralYear { get; set; }
    }
}
