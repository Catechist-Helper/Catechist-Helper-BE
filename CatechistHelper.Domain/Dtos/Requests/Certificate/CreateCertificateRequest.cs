using CatechistHelper.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Certificate
{
    public class CreateCertificateRequest
    {
        [Required(ErrorMessage = MessageConstant.Certificate.Require.NameRequired)]
        public string Name { get; set; } = null!;
        public IFormFile? Image { get; set; }
        public string? Description { get; set; }
        public Guid LevelId { get; set; }
    }
}
