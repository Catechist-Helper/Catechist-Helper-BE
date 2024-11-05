using System.ComponentModel.DataAnnotations;
using CatechistHelper.Domain.Constants;

namespace CatechistHelper.Domain.Dtos.Requests.Grade
{
    public class CreateGradeRequest
    {
        [Required(ErrorMessage = MessageConstant.Grade.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Vượt quá {0} kí tự")]
        public string Name { get; set; } = null!;

        public Guid MajorId { get; set; }
    }
}
