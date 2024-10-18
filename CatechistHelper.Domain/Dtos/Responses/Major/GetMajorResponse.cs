namespace CatechistHelper.Domain.Dtos.Responses.Major
{
    public class GetMajorResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public GetMajorResponse(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public GetMajorResponse()
        {
        }
    }
}
