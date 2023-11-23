using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Opportunities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.OpportunityAppServices.Dto
{
    [AutoMapTo(typeof(Opportunity))]
    public class OpportunityUpdateDto:EntityDto
    {
        public virtual string CompanyName { get; set; }
        public virtual string PersonName { get; set; }
        public virtual string EmailId { get; set; }
        public virtual string MobileNumber { get; set; }
        public string[] ProjectType { get; set; }
        public virtual int CalllCategoryId { get; set; }
        public virtual int AssignUserId { get; set; }
        public virtual string Comment { get; set; }
        public virtual DateTime? nextactiondate { get; set; }
        public virtual decimal ProjectValue { get; set; }
        public virtual int BeneficiaryCompanyId { get; set; }
    }
}
