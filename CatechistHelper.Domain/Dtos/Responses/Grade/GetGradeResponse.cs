using CatechistHelper.Domain.Dtos.Responses.Major;
using CatechistHelper.Domain.Dtos.Responses.PastoralYear;

namespace CatechistHelper.Domain.Dtos.Responses.Grade
{
    public class GetGradeResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalCatechist { get; set; }
        public GetMajorResponse Major { get; set; } 

        public GetGradeResponse(Guid id, string name, int totalCatechist, GetMajorResponse major)
        {
            Id = id;
            Name = name;
            TotalCatechist = totalCatechist;
            Major = major;
        }

        public GetGradeResponse()
        {
        }
    }
}
