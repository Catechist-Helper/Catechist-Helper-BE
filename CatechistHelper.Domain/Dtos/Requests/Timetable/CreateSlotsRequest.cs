namespace CatechistHelper.Domain.Dtos.Requests.Timetable
{
    public class CreateSlotsRequest
    {
        public Guid ClassId { get; set; }
        public Guid RoomId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<CatechistSlot> Catechists { get; set; } = [];
    }


    public class CatechistSlot
    {
        public Guid CatechistId { get; set; }
        public bool IsMain { get; set; }
    }
}
