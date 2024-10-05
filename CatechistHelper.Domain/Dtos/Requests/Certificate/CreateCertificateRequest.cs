using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Certificate
{
    public class CreateCertificateRequest
    {
        [Required(ErrorMessage = MessageConstant.Certificate.Require.NameRequired)]
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public Guid LevelId { get; set; }


    }
}
