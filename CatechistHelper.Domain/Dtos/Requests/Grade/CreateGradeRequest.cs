using CatechistHelper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatechistHelper.Domain.Constants;

namespace CatechistHelper.Domain.Dtos.Requests.Grade
{
    public class CreateGradeRequest
    {
        [Required(ErrorMessage = MessageConstant.Grade.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Vượt quá {0} kí tự")]
        public string Name { get; set; } = null!;

        public Guid MajorId { get; set; }

        public Guid PastoralYearId { get; set; }

    }
}
