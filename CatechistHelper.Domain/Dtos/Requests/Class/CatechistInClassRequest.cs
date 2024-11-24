using CatechistHelper.Domain.Dtos.Requests.Timetable;

namespace CatechistHelper.Domain.Dtos.Requests.Class
{
    public class CatechistInClassRequest
    {
        public List<CatechistSlot> Catechists { get; set; } = [];
    }

    public class RoomOfClassRequest
    {
        public Guid RoomId { get; set; }
    }
}
