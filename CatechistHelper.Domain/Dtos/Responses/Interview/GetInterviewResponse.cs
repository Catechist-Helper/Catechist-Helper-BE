using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Entities;
using System.Text.Json.Serialization;

namespace CatechistHelper.Domain.Dtos.Responses.Interview
{
    public class RecruiterInInterviewReponse
    {
        public Guid AccountId { get; set; }
        public string? OnlineRoomUrl { get; set; }

    }

    public class GetInterviewResponse
    {
        public Guid Id { get; set; }
        public DateTime MeetingTime { get; set; }
        public string Note { get; set; }
        public bool IsPassed { get; set; }
        public InterviewType? InterviewType { get; set; }
        public List<RecruiterInInterviewReponse> RecruiterInInterviews { get; set; }
        [JsonPropertyName("recruiters")]
        public List<GetAccountResponse> Accounts { get; set; }
    }
}
