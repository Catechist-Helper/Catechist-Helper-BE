namespace CatechistHelper.Domain.Dtos.Requests.Level
{
    public class CreateLevelRequest
    {
        public string Description { get; set; } = string.Empty;
        public int CatechismLevel { get; set; }
    }
}
