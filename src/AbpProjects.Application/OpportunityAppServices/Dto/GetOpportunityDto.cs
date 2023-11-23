using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using AbpProjects.Opportunities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project.Dto
{
    [AutoMapFrom(typeof(Opportunity))]
    public class GetOpportunityDto : EntityDto
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
        public virtual DateTime? nextactiondate { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? ActionDate { get; set; }
        public virtual int OpportunityOwner { get; set; }
        public virtual string OpportunityOwnerName { get; set; }
        public virtual string UploaderName { get; set; }
        public virtual int? FollowUpCount { get; set; }

        public virtual decimal ProjectValue { get; set; }
        public virtual decimal? ClosedAmount { get; set; }
        public virtual DateTime? ExpectedClosingDate { get; set; }
        public virtual string Reason { get; set; }
        public virtual int BeneficiaryCompanyId { get; set; }
        public virtual string BeneficiaryCompany { get; set; }
        public virtual int FollowUpTypeId { get; set; }
        public virtual string FollowUpType { get; set; }
    }

    public class ProjectTypeName
    {
        public virtual string Name { get; set; }
    }

    public class GetOpportunityInputDto : PagedAndSortedResultRequestDto
    {
        public int? CalllCategoryId { get; set; }
        public int? AssignUserId { get; set; }
        public string CompanyName { get; set; }
        public int? CurrentUser { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public virtual string PersonName { get; set; }
        public virtual string MobileNumber { get; set; }
        public int? BeneficiaryCompanyId { get; set; }
        //public void Normalize()
        //{
        //    if (string.IsNullOrEmpty(Sorting))
        //    {
        //        Sorting = "CompanyName";
        //    }
        //}
    }

    public class GetOpportunityExportInputDto: PagedAndSortedResultRequestDto
    {
        public int? CalllCategoryId { get; set; }
        public int? AssignUserId { get; set; }

        public string CompanyName { get; set; }
        public int? CurrentUser { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

}
