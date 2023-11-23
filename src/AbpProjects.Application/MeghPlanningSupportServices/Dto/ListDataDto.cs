using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.MeghPlanningSupports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningSupportServices.Dto
{
    public class GetServiceInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public int? ServiceId { get; set; }
        public int? ClientId { get; set; }
        public int? EmployeeId { get; set; }
        public bool? Cancelflag { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string DomainName { get; set; }
        public virtual DateTime NextRenewalDate { get; set; }
        public int? TypeName { get; set; }
    }
    [AutoMapFrom(typeof(ManageService))]
    public class ListDataDto : EntityDto
    {
        public virtual int ServiceId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string DomainName { get; set; }
        public virtual decimal Price { get; set; }
        public virtual DateTime NextRenewalDate { get; set; }
        public virtual string Comment { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual string HostingSpace { get; set; }
        public virtual int? ServerType { get; set; }
        public virtual int? TypeName { get; set; }
        public virtual int? NoOfEmail { get; set; }
        public virtual string ServiceName { get; set; }
        public virtual int Sid { get; set; }
        public virtual string displayTypename1 { get; set; }
        public virtual string ClientName { get; set; }
        public virtual int Cid { get; set; }
        public virtual bool IsAutoRenewal { get; set; }
        public virtual string EmployeeName { get; set; }
        public virtual int Eid { get; set; }
        public virtual DateTime? Createtime { get; set; }
        public virtual string ServerName { get; set; }
        public virtual bool? Cancelflag { get; set; }
        public virtual DateTime? CancelDate { get; set; }
        public virtual DateTime? RenewalDate { get; set; }
        public virtual int? TypeId { get; set; }
        public virtual string Typeofssl { get; set; }
        public virtual string Title { get; set; }
        public virtual int Term { get; set; }
        public virtual string DisplayTypename { get; set; }
        public virtual DateTime? RegistrationDate { get; set; }
        public virtual decimal AdjustmentAmount { get; set; }
        public virtual bool checkadjustment { get; set; }
        public virtual bool checkrenew { get; set; }
        public virtual int Actionstatus { get; set; }
        public virtual int Status { get; set; }
        public virtual int StatusName { get; set; }
        public virtual int? Credits { get; set; }
        public virtual string ActionName { get; set; }
        public virtual string DatabaseSpace { get; set; }
    }
}
