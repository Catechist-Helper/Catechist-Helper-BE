using CatechistHelper.Domain.Dtos.Responses.Account;

namespace CatechistHelper.Domain.Dtos.Responses.Member
{
    public class GetMemberResponse
    {
        public required GetAccountResponse GetAccountResponse { get; set; }
        public Guid RoleEventId { get; set; }
    }
}
