using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Major
{
    public class CreateMajorRequest
    {
        [Required(ErrorMessage = MessageConstant.Major.Require.NameRequired)]
        [MaxLength(20, ErrorMessage = "Vượt quá {1} kí tự!")]
        public string Name { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = MessageConstant.Common.PositiveNumberError)]
        public int HierarchyLevel { get; set; }
    }
}
