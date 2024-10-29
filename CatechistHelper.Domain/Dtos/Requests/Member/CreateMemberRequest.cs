namespace CatechistHelper.Domain.Dtos.Requests.Member
{
    public class CreateMemberRequest
    {
        public Guid Id { get; set; }
        public Guid RoleEventId { get; set; }
    }
}
