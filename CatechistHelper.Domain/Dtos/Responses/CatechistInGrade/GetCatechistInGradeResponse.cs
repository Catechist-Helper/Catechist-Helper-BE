using CatechistHelper.Domain.Dtos.Responses.Catechist;

namespace CatechistHelper.Domain.Dtos.Responses.CatechistInGrade
{
    public class GetCatechistInGradeResponse
    {
        public required GetCatechistResponse Catechist { get; set; }
        public bool IsMain { get; set; }
    }
}
