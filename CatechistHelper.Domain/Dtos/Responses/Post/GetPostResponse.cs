namespace CatechistHelper.Domain.Dtos.Responses.Post
{
    public class GetPostResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid AccountId { get; set; }
        public Guid PostCategoryId { get; set; }
    }
}
