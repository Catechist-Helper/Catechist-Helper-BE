using CatechistHelper.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Room
{
    public class CreateRoomRequest
    {
        [Required(ErrorMessage = MessageConstant.Room.Require.NameRequired)]
        [MaxLength(10, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Name { get; set; } = null!;

        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
    }
}
