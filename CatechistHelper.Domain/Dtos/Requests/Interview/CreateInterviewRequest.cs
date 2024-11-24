using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Interview
{
    public class CreateInterviewRequest
    {
        public Guid RegistrationId { get; set; }
        public DateTime MeetingTime { get; set; }
        public InterviewType InterviewType { get; set; } = InterviewType.Offline;
    }
}
