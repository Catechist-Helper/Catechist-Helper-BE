using CatechistHelper.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Room
{
    public class CreateRoomRequest
    {
        [Required(ErrorMessage = MessageConstant.Room.Require.NameRequired)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
