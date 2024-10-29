namespace CatechistHelper.Domain.Dtos.Responses.RoleEvent
{
    public class GetRoleEventResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
    }
}
