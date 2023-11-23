using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Opportunities
{
    [Table("FollowupType")]
    public class Followuptype : FullAuditedEntity
    {
        [Required]
        public virtual string FollowUpType { get; set; }
    }
}
