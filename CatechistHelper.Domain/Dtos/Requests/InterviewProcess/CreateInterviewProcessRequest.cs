using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.InterviewProcess
{
    public class CreateInterviewProcessRequest
    {
        [Required]
        public Guid RegistrationId { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
