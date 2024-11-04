using CatechistHelper.Domain.Dtos.Responses.CatechistInSlot;
using CatechistHelper.Domain.Dtos.Responses.Room;

namespace CatechistHelper.Domain.Dtos.Responses.Slot
{
    public class GetSlotResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }
        public required GetRoomResponse Room { get; set; }
        public required List<GetCatechistInSlotResponse> CatechistInSlots { get; set; }
    }
}
