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
    [AutoMapFrom(typeof(Opportunity))]
    public class GetOpportunityReportDto : EntityDto
    {
        public virtual string CompanyName { get; set; }
        public virtual string PersonName { get; set; }
        public virtual string EmailId { get; set; }
        public virtual string MobileNumber { get; set; }
        public virtual int CalllCategoryId { get; set; }
        public virtual string CallCategoryName { get; set; }
        public virtual int AssignUserId { get; set; }
        public virtual string AssignUserName { get; set; }
        public virtual string Comment { get; set; }
        public virtual int[] ProjectTypeName { get; set; }
        public virtual List<string> ProjectType_Name { get; set; }
        public virtual int CreateUser { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual decimal ProjectValue { get; set; }
        public virtual decimal? ClosedAmount { get; set; }
        public virtual DateTime? ExpectedClosingDate { get; set; }
        public virtual string Reason { get; set; }
        public virtual int BeneficiaryCompanyId { get; set; }
        public virtual string BeneficiaryCompany { get; set; }
    }

    [AutoMapFrom(typeof(Opportunity))]
    public class GetDailySalesActivityReportDto : EntityDto
    {
        public virtual string CompanyName { get; set; }
        public virtual string PersonName { get; set; }
        public virtual string EmailId { get; set; }
        public virtual string MobileNumber { get; set; }
        public virtual int CalllCategoryId { get; set; }
        public virtual string CallCategoryName { get; set; }
        public virtual int AssignUserId { get; set; }
        public virtual string AssignUserName { get; set; }
        public virtual string Comment { get; set; }
        public virtual int[] ProjectTypeName { get; set; }
        public virtual List<string> ProjectType_Name { get; set; }
        public virtual int CreateUser { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual decimal ProjectValue { get; set; }
        public virtual int FollowupCount { get; set; }
    }

    public class GetFollowUpCountDto : EntityDto
    {
        public virtual string Name { get; set; }
        public virtual int count { get; set; }
    }
    public class GetFollowUpFilterDto
    {
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }
    }
}
