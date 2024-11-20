namespace CatechistHelper.Domain.Dtos.Responses.ChristianName
{
    public class GetChristianNameResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Gender { get; set; } = string.Empty;
        public DateTime? HolyDay { get; set; }
    }
}
