namespace CatechistHelper.Domain.Dtos.Responses.Level
{
    public class GetLevelResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CatechismLevel { get; set; }
    }
}
