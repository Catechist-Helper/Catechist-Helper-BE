namespace CatechistHelper.Domain.Dtos.Responses.Level
{
    public class GetLevelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CatechismLevel { get; set; }
    }
}
