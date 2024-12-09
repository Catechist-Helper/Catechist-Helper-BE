namespace CatechistHelper.Domain.Dtos.Responses.Timetable
{
    public class CalendarResponse
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Start { get; set; } = string.Empty;
        public string? End { get; set; }
    }
}
