﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("system_configuration")]
    public class SystemConfiguration
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("key")]
        [StringLength(100)] 
        public string Key { get; set; } = null!;

        [Column("value")]
        public string Value { get; set; } = null!;
    }
}
