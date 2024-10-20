namespace CatechistHelper.Domain.Dtos.Requests.CatechistInClass
{
    public class CreateCatechistInClassRequest
    {
        public Guid ClassId { get; set; }
        public Guid CatechistId { get; set; }
        public bool IsMain { get; set; }
    }
}
