namespace CatechistHelper.Domain.Dtos.Responses.EventCategory
{
    public class GetEventCategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
    }
}
