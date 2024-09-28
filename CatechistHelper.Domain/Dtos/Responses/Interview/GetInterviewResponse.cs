namespace CatechistHelper.Domain.Dtos.Responses.Interview
{
    public class GetInterviewResponse
    {
        public Guid Id { get; set; }
        public DateTime MeetingTime { get; set; }
        public string Note { get; set; }
        public bool IsPassed { get; set; }
    }
}
