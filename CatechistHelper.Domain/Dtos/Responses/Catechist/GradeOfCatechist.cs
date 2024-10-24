using CatechistHelper.Domain.Dtos.Responses.Grade;

namespace CatechistHelper.Domain.Dtos.Responses.Catechist
{
    public class GradeOfCatechist
    {
        public GetGradeResponse? Grade { get; set; }
        public bool IsMain { get; set; }

    }
}
