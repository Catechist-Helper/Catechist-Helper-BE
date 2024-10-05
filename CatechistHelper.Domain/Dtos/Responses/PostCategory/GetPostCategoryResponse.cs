namespace CatechistHelper.Domain.Dtos.Responses.PostCategory
{
    public class GetPostCategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
