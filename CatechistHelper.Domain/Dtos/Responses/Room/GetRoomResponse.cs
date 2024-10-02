namespace CatechistHelper.Domain.Dtos.Responses.Room
{
    public class GetRoomResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
    }
}
