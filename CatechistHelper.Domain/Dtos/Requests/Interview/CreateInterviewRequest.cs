using CatechistHelper.Domain.Entities;

namespace CatechistHelper.Domain.Dtos.Requests.Interview
{
    public class CreateInterviewRequest
    {
        public Guid RegistrationId { get; set; }
        public DateTime MeetingTime { get; set; }
        public List<Guid>? Accounts { get; set; }
        public InterviewType InterviewType { get; set; } = InterviewType.Offline;
    }
}
