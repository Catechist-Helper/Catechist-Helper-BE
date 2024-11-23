using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Level
{
    public class CreateLevelRequest
    {
        [Required(ErrorMessage = MessageConstant.Level.Require.NameRequired)]
        [MaxLength(10, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Name { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = MessageConstant.Common.PositiveNumberError)]
        public int HierarchyLevel { get; set; }
    }
}
