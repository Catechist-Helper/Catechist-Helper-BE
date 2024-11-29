using System;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Class
{
    public class ClassRequest
    {
        [Required(ErrorMessage = "Class name is required.")]
        [StringLength(100, ErrorMessage = "Class name must be less than 100 characters.")]
        public string Name { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Number of catechists must be at least 1.")]
        public int NumberOfCatechist { get; set; } = 1;

        [MaxLength(500, ErrorMessage = "Note cannot be longer than 500 characters.")]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Pastoral Year ID is required.")]
        public Guid PastoralYearId { get; set; }

        [Required(ErrorMessage = "Grade ID is required.")]
        public Guid GradeId { get; set; }
    }

}
