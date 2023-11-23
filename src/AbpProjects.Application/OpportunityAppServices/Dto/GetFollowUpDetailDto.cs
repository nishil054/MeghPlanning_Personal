using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.OpportunityAppServices.Dto
{
    public class GetFollowUpDetailDto:EntityDto
    {
        public virtual int opporutnityid { get; set; }
        public virtual DateTime? nextactiondate { get; set; }
        public virtual DateTime? expectedclosingdate { get; set; }
        public virtual string Comment { get; set; }
        public virtual int CalllCategoryId { get; set; }
        public virtual int[] ProjectTypeName { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string PersonName { get; set; }
        public virtual string EmailId { get; set; }
        public virtual string MobileNumber { get; set; }
        public virtual int BeneficiaryCompanyId { get; set; }
        public virtual string BeneficiaryCompany { get; set; }
    }
}
