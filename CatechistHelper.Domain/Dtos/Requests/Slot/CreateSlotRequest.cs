namespace CatechistHelper.Domain.Dtos.Requests.Slot
{
    public class CreateSlotRequest
    {
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }
        public Guid ClassId { get; set; }
        public Guid RoomId { get; set; }

    }
}
