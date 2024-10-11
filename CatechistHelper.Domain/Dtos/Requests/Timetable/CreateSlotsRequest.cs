namespace CatechistHelper.Domain.Dtos.Requests.Timetable
{
    public class CreateSlotsRequest
    {
        public Guid ClassId { get; set; }
        public Guid RoomId { get; set; }
        public int Hour { get; set; }
    }
}
