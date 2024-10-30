using CatechistHelper.Domain.Dtos.Responses.Account;
namespace CatechistHelper.Domain.Dtos.Responses.MemberOfProcess
{
    public class GetMemberOfProcessRepsonse
    {
        public required GetAccountResponse GetAccountResponse {  get; set; }
        public bool IsMain { get; set; }
    }
}
