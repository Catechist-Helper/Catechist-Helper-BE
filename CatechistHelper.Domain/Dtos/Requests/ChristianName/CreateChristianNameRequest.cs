using System.ComponentModel.DataAnnotations;
using CatechistHelper.Domain.Constants;

namespace CatechistHelper.Domain.Dtos.Requests.ChristianName
{
    public class CreateChristianNameRequest
    {
        [Required(ErrorMessage = MessageConstant.ChristianName.Require.NameRequired)]
        [StringLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Name { get; set; } = null!;

        [MaxLength(10, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Gender { get; set; }

        public DateTime? HolyDay { get; set; }
    }
}
