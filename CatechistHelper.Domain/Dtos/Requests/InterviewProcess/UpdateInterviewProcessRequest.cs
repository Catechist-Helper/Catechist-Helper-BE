using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.InterviewProcess
{
    public class UpdateInterviewProcessRequest
    {
        [Required(ErrorMessage = MessageConstant.InterviewProcess.Require.NameRequired)]
        [MaxLength(20, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Name { get; set; } = null!;
        public InterviewProcessStatus Status { get; set; }
    }
}
