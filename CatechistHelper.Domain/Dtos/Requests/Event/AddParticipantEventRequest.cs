using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CatechistHelper.Domain.Dtos.Requests.Event
{
    public class AddParticipantEventRequest
    {
        [Required]
        public Guid EventId { get; set; }
        public IFormFile File { get; set; }
    }
}
