using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Interview
{
    public class CreateInterviewRequest
    {
        [Required(ErrorMessage = MessageConstant.Interview.Require.RegistrationIdRequired)]
        public Guid RegistrationId { get; set; }

        [Required(ErrorMessage = MessageConstant.Interview.Require.MeetingTimeRequired)]
        public DateTime MeetingTime { get; set; }
    }
}
