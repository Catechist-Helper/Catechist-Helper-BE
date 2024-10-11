using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Timetable
{
    public class CreateTimetableRequest
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
