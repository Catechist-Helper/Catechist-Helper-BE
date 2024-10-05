using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Room
{
    public class CreateRoomRequest
    {
        [Required(ErrorMessage = MessageConstant.Room.Require.NameRequired)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }

    }
}
