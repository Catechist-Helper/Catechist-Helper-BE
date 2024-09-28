using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("catechist_in_class")]
    public class CatechistInClass
    {
        [Column("class_id")]
        public Guid ClassId { get; set; }
        public virtual Class Class { get; set; } = null!;

        [Column("catechist_id")]
        public Guid CatechistId { get; set; }
        public virtual Catechist Catechist { get; set; } = null!;

        [Column("is_main")]
        public bool IsMain { get; set; }
    }
}
