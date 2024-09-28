using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Responses.InterviewProcess
{
    public class GetInterviewProcessResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public InterviewProcessStatus Status { get; set; }
    }
}
