using Newtonsoft.Json;

namespace CatechistHelper.Domain.Dtos.Responses.Timetable
{
    public class HolidayResponse
    {
        [JsonProperty("items")]
        public List<CalendarEvent> Items { get; set; } = [];
    }

    public class CalendarEvent
    {
        [JsonProperty("summary")]
        public string Summary { get; set; } = string.Empty;
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;
        [JsonProperty("start")]
        public EventDate Start { get; set; } = null!;
        [JsonProperty("end")]
        public EventDate End { get; set; } = null!;

    }

    public class EventDate
    {
        public string Date { get; set; } = string.Empty;
    }
}
