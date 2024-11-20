using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.EventCategory
{
    public class CreateEventCategoryRequest
    {
        [Required(ErrorMessage = MessageConstant.EventCategory.Require.NameRequired)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
