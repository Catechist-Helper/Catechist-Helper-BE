using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Interview
{
    public class UpdateInterviewRequest
    {
        public DateTime MeetingTime { get; set; }

        public string? Note { get; set; }

        public bool IsPassed { get; set; }

        public string? Reason { get; set; }
    }
}
