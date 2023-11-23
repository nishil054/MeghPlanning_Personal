using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Opportunities
{
    public class Opportunity: FullAuditedEntity
    {
        public virtual string CompanyName { get; set; }
        public virtual string PersonName { get; set; }
        public virtual string EmailId { get; set; }
        public virtual string MobileNumber { get; set; }
        public virtual int CalllCategoryId { get; set; }
        public virtual int AssignUserId { get; set; }
        public virtual string Comment { get; set; }
        public virtual DateTime? ActionDate { get; set; }
        public virtual string Remarks { get; set; }
        public int OpportunityOwner { get; set; }
        public virtual decimal ProjectValue { get; set; }
        [Required]
        public virtual int BeneficiaryCompanyId { get; set; }
    }
}
