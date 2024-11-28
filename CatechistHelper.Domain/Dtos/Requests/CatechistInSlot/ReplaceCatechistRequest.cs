using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Requests.CatechistInSlot
{
    public class ReplaceCatechistRequest
    {
        public Guid CurrentId { get; set; }
        public Guid ReplaceId { get; set; }
        public CatechistInSlotType Type { get; set; }
    }
}
