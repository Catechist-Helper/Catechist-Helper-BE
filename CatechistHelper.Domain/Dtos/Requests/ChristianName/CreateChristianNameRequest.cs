using System.ComponentModel.DataAnnotations;
using CatechistHelper.Domain.Constants;

namespace CatechistHelper.Domain.Dtos.Requests.ChristianName
{
    public class CreateChristianNameRequest
    {
        [Required(ErrorMessage = MessageConstant.ChristianName.Require.NameRequired)]
        [StringLength(50, ErrorMessage = "Vượt quá {1} kí tự!")]
        public string Name { get; set; } = null!;
        public string? Gender { get; set; }
        public DateTime? HolyDay { get; set; }
    }
}
