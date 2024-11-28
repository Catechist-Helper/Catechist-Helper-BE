using CatechistHelper.Domain.Dtos.Responses.Account;
using System.Text.Json.Serialization;

namespace CatechistHelper.Domain.Dtos.Responses.Interview
{
    public class GetInterviewResponse
    {
        public Guid Id { get; set; }
        public DateTime MeetingTime { get; set; }
        public string Note { get; set; }
        public bool IsPassed { get; set; }
        [JsonPropertyName("recruiters")]
        public List<GetAccountResponse> Accounts { get; set; }
    }
}
