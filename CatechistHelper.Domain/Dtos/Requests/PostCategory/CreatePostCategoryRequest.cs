using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.PostCategory
{
    public class CreatePostCategoryRequest
    {
        [Required(ErrorMessage = MessageConstant.PostCategory.Require.NameRequired)]
        public string Name { get; set; } = null;

        public string Description { get; set; }

    }
}
