namespace CatechistHelper.Domain.Dtos.Requests.Slot
{

    public class UpdateSlotRequest
    {
        public List<CatechistSlotUpdate> CatechistInSlots { get; set; } = [];
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid? RoomId { get; set; }
        public bool? IsDeletedRoom { get; set; }
    }

    public class CatechistSlotUpdate
    {
        public Guid CatechistId { get; set; }
        public bool IsMain { get; set; }
    }
}
