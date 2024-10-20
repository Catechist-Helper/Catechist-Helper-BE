namespace CatechistHelper.Domain.Dtos.Requests.CatechistInClass
{
    public class UpdateCatechistInClassRequest
    {
        public Guid CatechistId { get; set; }
        public bool IsMain { get; set; }
    }
}
