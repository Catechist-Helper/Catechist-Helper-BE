namespace CatechistHelper.Domain.Dtos.Requests.Level
{
    public class CreateLevelRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int HierarchyLevel { get; set; }
    }
}
