using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project
{
    [Table("Project")]
  public class project : FullAuditedEntity
    {
        public const int maxLengthProjectName = 250;
        [Required]
        [MaxLength(maxLengthProjectName)]
        public virtual string ProjectName { get; set; }

        public const int maxLengthDescription = 500;
        [Required]
        [MaxLength(maxLengthDescription)]
        public virtual string Description { get; set; }

        [Required]
        public virtual int BeneficiaryCompanyId { get; set; }
      
        [Required]
        public virtual DateTime StartDate { get; set; }

        
        public virtual DateTime? EndDate { get; set; }

       
        public virtual DateTime? TeamDeadline { get; set; }
        public virtual DateTime? ActualEndDate { get; set; }

        public const int maxLengthCompanyName = 250;
       
        public virtual string CompanyName { get; set; }

        public virtual int? Marketing_LeaderId { get; set; }

        public virtual decimal Price { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int Status { get; set; }
        public virtual decimal totalhours { get; set; }
        public virtual int? Priority { get; set; }
        public virtual int IsEnable { get; set; } = 0;
        public virtual int Opportunityid { get; set; } = 0;
    }
}
