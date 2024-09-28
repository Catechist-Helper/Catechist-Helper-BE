using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Requests.InterviewProcess
{
    public class UpdateInterviewProcessRequest
    {
        public string? Name {  get; set; }
        public InterviewProcessStatus Status { get; set; }
    }
}
