namespace CatechistHelper.Domain.Dtos.Requests.ChristianName
{
    public class UpdateChristianNameRequest
    {
        public string? Name { get; set; } = null!;
        public string? Gender { get; set; }
        public DateTime? HolyDay { get; set; }
    }
}
