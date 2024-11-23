using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Interview
{
    public class UpdateInterviewRequest
    {
        public DateTime MeetingTime { get; set; }

        [MaxLength(500, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Note { get; set; }

        public bool IsPassed { get; set; }

        public string? Reason { get; set; }
    }
}
