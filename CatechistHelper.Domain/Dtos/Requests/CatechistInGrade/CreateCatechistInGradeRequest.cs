namespace CatechistHelper.Domain.Dtos.Requests.CatechistInGrade
{
    public class CreateCatechistInGradeRequest
    {
        public Guid GradeId { get; set; }
        public required List<Guid> CatechistIds { get; set; }
        public Guid MainCatechistId { get; set; }
    }
}
