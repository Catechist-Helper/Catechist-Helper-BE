﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatechistHelper.Domain.Entities
{
    [Table("role_event")]
    public class RoleEvent
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [InverseProperty(nameof(RoleEvent))]
        public ICollection<Member> Members { get; set; } = new List<Member>();
    }
}