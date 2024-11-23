using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.EventCategory
{
    public class CreateEventCategoryRequest
    {
        [Required(ErrorMessage = MessageConstant.EventCategory.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Name { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Description { get; set; }
    }
}
