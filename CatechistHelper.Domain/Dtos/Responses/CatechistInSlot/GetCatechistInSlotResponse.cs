using CatechistHelper.Domain.Dtos.Responses.Catechist;

namespace CatechistHelper.Domain.Dtos.Responses.CatechistInSlot
{
    public class GetCatechistInSlotResponse
    {
        public required GetCatechistResponse Catechist { get; set; }
        public string Type { get; set; }
    }
}
