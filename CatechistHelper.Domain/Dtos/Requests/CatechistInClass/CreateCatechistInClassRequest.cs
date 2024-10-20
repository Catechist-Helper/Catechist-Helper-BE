namespace CatechistHelper.Domain.Dtos.Requests.CatechistInClass
{
    public class CreateCatechistInClassRequest
    {
        public Guid ClassId { get; set; }
        public required List<Guid> CatechistIds { get; set; }
        public Guid MainCatechistId { get; set; }
    }
}
