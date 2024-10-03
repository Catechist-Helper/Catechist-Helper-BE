namespace CatechistHelper.Domain.Dtos.Responses.Room
{
    public class GetRoomResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}
