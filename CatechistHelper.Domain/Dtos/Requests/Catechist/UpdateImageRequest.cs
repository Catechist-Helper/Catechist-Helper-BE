using Microsoft.AspNetCore.Http;

namespace CatechistHelper.Domain.Dtos.Requests.Catechist
{
    public class UpdateImageRequest
    {
        public IFormFile? ImageUrl { get; set; }
    }
}
