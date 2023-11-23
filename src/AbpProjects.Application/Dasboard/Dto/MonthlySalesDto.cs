using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Dasboard.Dto
{
   // [AutoMapFrom(typeof(project))]
    public  class MonthlySalesDto : EntityDto
    {
        public virtual int MarketingLeaderId { get; set; }
        public virtual string MarketingLeaderName { get; set; }
        public virtual int?  NoOfSales { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual int ProjectTypeId { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal TargetAmount { get; set; }
        public virtual int month { get; set; }

        public virtual string monthname { get; set; }

    }
    public class MonthlySalesreportDto : EntityDto
    {
        public virtual string y { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal TargetAmount { get; set; }  
       
    }
    public class morrisDto
    {
        public virtual int a { get; set; }
        public virtual string y { get; set; }
    }
    }
