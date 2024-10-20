namespace CatechistHelper.Domain.Dtos.Requests.CatechistInGrade
{
    public class CreateCatechistInGradeRequest
    {
        public Guid GradeId { get; set; }
        public required List<CatechistInGradeRequest> Catechists { get; set; }
    }
}
