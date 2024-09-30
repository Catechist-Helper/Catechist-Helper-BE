using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.PostCategory
{
    public class CreatePostCategoryRequest
    {
        [Required(ErrorMessage = MessageConstant.PostCategory.Require.NameRequired)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = MessageConstant.PostCategory.Require.DescriptionRequired)]
        public string Description { get; set; } = string.Empty;

    }
}
