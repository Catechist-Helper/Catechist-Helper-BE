using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Post
{
    public class UpdatePostRequest
    {
        [Required(ErrorMessage = MessageConstant.Post.Require.TitleRequired)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Post.Require.ContentRequired)]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Post.Require.ModuleRequired)]

        public string Module { get; set; } = null!;

        public Guid AccountId { get; set; }

        [Required(ErrorMessage = MessageConstant.Post.Require.PostCategoryRequired)]
        public Guid PostCategoryId { get; set; }
    }
}
