namespace CatechistHelper.Domain.Dtos.Responses.CatechistInSlot
{
    public class SearchCatechistResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ChristianName { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
    }
}
