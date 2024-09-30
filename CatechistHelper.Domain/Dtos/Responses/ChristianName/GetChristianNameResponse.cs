namespace CatechistHelper.Domain.Dtos.Responses.ChristianName
{
    public class GetChristianNameResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime HolyDay { get; set; }
    }
}
