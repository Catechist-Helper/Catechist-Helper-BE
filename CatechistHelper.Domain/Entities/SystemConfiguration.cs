using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatechistHelper.Domain.Entities
{
    [Table("system_configuration")]
    public class SystemConfiguration
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("key")]
        public string Key { get; set; } = null!;

        [Column("value")]
        public string Value { get; set; } = null!;
    }
}
