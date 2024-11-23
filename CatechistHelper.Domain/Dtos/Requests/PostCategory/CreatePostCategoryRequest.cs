using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.PostCategory
{
    public class CreatePostCategoryRequest
    {
        [Required(ErrorMessage = MessageConstant.PostCategory.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Name { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Description { get; set; }
    }
}
