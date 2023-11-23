using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Opportunities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    public class GetDailyActivityReportDto : EntityDto
    {
        public virtual int OpportunityId { get; set; }
        public virtual DateTime FollowUpDate { get; set; }
        public virtual string Date { get; set; }
        public virtual List<UserDetails> UserData { get; set; }
    }
    public class UserDetails
    {
        public int UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual List<OppDetails> OppData { get; set; }
        public virtual int FirstAtmptCount { get; set; }
        public virtual int TotalCount { get; set; }
        public virtual int OpportunityId { get; set; }

    }
    public class OppDetails
    {
        public virtual int OpportunityId { get; set; }
        public virtual int FirstAtmptCount { get; set; }
    }
}
