namespace CatechistHelper.Domain.Dtos.Responses.Post
{
    public class GetPostResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Module { get; set; }
        public Guid AccountId { get; set; }
        public Guid PostCategoryId { get; set; }
    }
}
