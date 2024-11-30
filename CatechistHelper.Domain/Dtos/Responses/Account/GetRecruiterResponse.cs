using CatechistHelper.Domain.Dtos.Responses.Interview;
using CatechistHelper.Domain.Dtos.Responses.Role;

namespace CatechistHelper.Domain.Dtos.Responses.Account
{
    public class GetRecruiterResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public RoleResponse? Role { get; set; }
        public List<GetInterviewResponse>? Interviews { get; set; }
    }
}
