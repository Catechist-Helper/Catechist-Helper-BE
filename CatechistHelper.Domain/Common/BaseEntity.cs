using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatechistHelper.Domain.Common
{
    public abstract class BaseEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [Column("created_by")]
        public string? CreatedBy { get; set; }
        [Column("updated_by")]
        public string? UpdatedBy { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
