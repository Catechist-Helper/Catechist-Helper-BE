using System.ComponentModel.DataAnnotations;
using CatechistHelper.Domain.Constants;

namespace CatechistHelper.Domain.Dtos.Requests.RoleEvent
{
    public class CreateRoleEventRequest
    {
        [Required(ErrorMessage = MessageConstant.RoleEvent.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Vượt quá {1} kí tự!")]
        public string Name { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Vượt quá {1} kí tự!")]
        public string? Description { get; set; }
    }
}
