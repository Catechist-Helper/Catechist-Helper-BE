using CatechistHelper.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Column("is_public")]
        [Required]
        public bool IsPublic { get; set; }

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
