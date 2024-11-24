using CatechistHelper.Domain.Dtos.Responses.EventCategory;
using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Responses.Event
{
    public class GetEventResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsPeriodic { get; set; }
        public bool IsCheckedIn { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double current_budget { get; set; } = 0;
        public EventStatus EventStatus { get; set; }
        public GetEventCategoryResponse? EventCategory { get; set; }

    }
}
