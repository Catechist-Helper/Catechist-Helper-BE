using CatechistHelper.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatechistHelper.Domain.Entities
{
    [Table("certificate_of_candidate")]
    public class CertificateOfCandidate : BaseEntity
    {
        [Column("image")]
        public string Image { get; set; } = null!;

        [Column("candidate_id")]
        [ForeignKey(nameof(Candidate))]
        public Guid CandidateId { get; set; } 
        public Candidate Candidate { get; set; } = null!;
    }
}
