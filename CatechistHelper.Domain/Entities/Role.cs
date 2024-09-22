using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatechistHelper.Domain.Entities
{
    [Table("role")]
    public class Role
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("role")]
        [StringLength(20)]
        [Required]
        public string RoleName { get; set; } = null!;

        [InverseProperty(nameof(Role))]
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
