using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Post
{
    public class CreatePostRequest
    {
        [Required(ErrorMessage = MessageConstant.Post.Require.TitleRequired)]
        [MaxLength(100, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Post.Require.ContentRequired)]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Post.Require.ModuleRequired)]
        [MaxLength(20, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Module { get; set; } = null!;

        public Guid AccountId { get; set; }

        public Guid PostCategoryId { get; set; }
    }
}
