using CatechistHelper.Domain.Dtos.Responses.Role;

namespace CatechistHelper.Domain.Dtos.Responses.Account
{
    public class GetAccountResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public RoleResponse Role { get; set; }
    }
}
