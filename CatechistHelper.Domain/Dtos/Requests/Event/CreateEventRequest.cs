using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Event
{
    public class CreateEventRequest
    {
        [Required(ErrorMessage = MessageConstant.Event.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Vượt quá {1} kí tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Event.Require.DescriptionRequired)]
        [MaxLength(1000, ErrorMessage = "Vượt quá {1} kí tự")]
        public string Description { get; set; } = null!;

        public bool IsPeriodic { get; set; }

        public bool IsCheckedIn { get; set; }

        [MaxLength(200, ErrorMessage = "Vượt quá {1} kí tự")]
        public string? Address { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public double current_budget { get; set; } = 0;

        public EventStatus EventStatus { get; set; }
    }
}
