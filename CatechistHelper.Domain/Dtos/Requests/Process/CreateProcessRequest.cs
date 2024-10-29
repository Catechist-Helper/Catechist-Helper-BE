using CatechistHelper.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatechistHelper.Domain.Constants;

namespace CatechistHelper.Domain.Dtos.Requests.Process
{
    public class CreateProcessRequest
    {
        [Required(ErrorMessage = MessageConstant.Process.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Vượt quá {1} kí tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.Process.Require.DescriptionRequired)]
        public string Description { get; set; } = null!;

        public TimeSpan Duration { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public double Fee { get; set; }

        public ProcessStatus Status { get; set; }

        public Guid EventId { get; set; }
    }
}
