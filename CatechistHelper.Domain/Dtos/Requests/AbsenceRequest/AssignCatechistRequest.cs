namespace CatechistHelper.Domain.Dtos.Requests.AbsenceRequest
{
    public class AssignCatechistRequest
    {
        public Guid RequestId { get; set; }
        public Guid ReplacementCatechistId { get; set; }
    }
}
