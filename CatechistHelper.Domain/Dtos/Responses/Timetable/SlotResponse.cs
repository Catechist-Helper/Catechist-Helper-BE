namespace CatechistHelper.Domain.Dtos.Responses.Timetable
{
    public class SlotResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }
        public string? ClassId { get; set; }
        public string? ClassName { get; set; }
        public string? RoomName { get; set; }
    }
}
