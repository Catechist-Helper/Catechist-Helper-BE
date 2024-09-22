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
    [Table("interview")]
    public class Interview : BaseEntity
    { 
        [Column("meeting_time")]
        public DateTime MeetingTime { get; set; }

        [Column("note")]
        [StringLength(500)]
        public string? Note { get; set; }

        [Column("is_passed")]
        public bool IsPassed { get; set; }

        [Column("application_id")]
        [ForeignKey(nameof(Application))]
        public Guid ApplicationId { get; set; }
        public Application Application { get; set; } = null!;
    }
}
