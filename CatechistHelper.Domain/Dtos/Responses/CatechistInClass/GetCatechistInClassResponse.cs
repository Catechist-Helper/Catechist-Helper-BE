using CatechistHelper.Domain.Dtos.Responses.Catechist;

namespace CatechistHelper.Domain.Dtos.Responses.CatechistInClass
{
    public class GetCatechistInClassResponse
    {
        public required GetCatechistResponse Catechist { get; set; }
        public bool IsMain { get; set; }
    }
}
