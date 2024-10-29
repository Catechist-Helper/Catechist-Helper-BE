using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Responses.Process
{
    public class GetProcessResponse
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Fee { get; set; }
        public ProcessStatus Status { get; set; }
        public Guid EventId { get; set; }
    }
}
