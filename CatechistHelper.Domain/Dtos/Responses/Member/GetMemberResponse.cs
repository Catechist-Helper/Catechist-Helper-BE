using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.RoleEvent;

namespace CatechistHelper.Domain.Dtos.Responses.Member
{
    public class GetMemberResponse
    {
        public required GetAccountResponse Account { get; set; }
        public required GetRoleEventResponse RoleEvent { get; set; }
        public Guid RoleEventId { get; set; }
    }
}
