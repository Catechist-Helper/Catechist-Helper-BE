using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Slot
{
    public class CreateSlotRequest
    {
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [MaxLength(500, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Note { get; set; }
        public Guid ClassId { get; set; }
        public Guid RoomId { get; set; }

    }
}
