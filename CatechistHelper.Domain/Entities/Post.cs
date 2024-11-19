using CatechistHelper.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("post")]
    public class Post : BaseEntity
    {
        [Column("title")]
        [StringLength(100)]
        [Required]
        public string Title { get; set; } = null!;

        [Column("content")]
        [Required]
        public string Content { get; set; } = null!;

        [Column("module")]
        [StringLength(20)]
        [Required]
        public string Module { get; set; } = null!;

        [Column("account_id")]
        [ForeignKey(nameof(Account))]
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;

        [Column("post_category_id")]
        [ForeignKey(nameof(PostCategory))]
        public Guid PostCategoryId { get; set; }
        public virtual PostCategory PostCategory { get; set; } = null!;
    }
}
