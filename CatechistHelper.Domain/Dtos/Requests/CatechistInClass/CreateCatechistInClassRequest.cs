using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Requests.CatechistInClass
{
    public class CreateCatechistInClassRequest
    {
        public Guid ClassId { get; set; }
        public required List<Guid> CatechistIds { get; set; }
        public Guid MainCatechistId { get; set; }
    }


    public class ReplaceCatechistInClassRequest
    {
        public Guid CatechistId { get; set; }
        public Guid ClassId { get; set; }
        public Guid ReplaceCatechistId { get; set; }
        public CatechistInSlotType Type { get; set; }
    }
}
