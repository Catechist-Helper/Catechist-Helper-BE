using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.CertificateOfCandidate;
using CatechistHelper.Domain.Dtos.Responses.Interview;
using CatechistHelper.Domain.Dtos.Responses.InterviewProcess;
using CatechistHelper.Domain.Enums;
using System.Text.Json.Serialization;

namespace CatechistHelper.Domain.Dtos.Responses.Registration
{
    public class GetRegistrationResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } 
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } 
        public string Email { get; set; } 
        public string Phone { get; set; }
        public bool IsTeachingBefore { get; set; }
        public int YearOfTeaching { get; set; }
        public string? Note { get; set; }
        public RegistrationStatus Status { get; set; }
        public List<GetCertificateOfCandidateResponse> CertificateOfCandidates { get; set; }
        public List<GetInterviewResponse> Interviews { get; set; }
        public List<GetInterviewProcessResponse> InterviewProcesses { get; set; }
        [JsonPropertyName("recruiters")]
        public List<GetAccountResponse> Accounts { get; set; }
    }
}
