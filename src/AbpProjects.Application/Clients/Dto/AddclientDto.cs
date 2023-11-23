using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.MeghPlanningSupports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Clients.Dto
{
    [AutoMapTo(typeof(Client))]
    public class AddclientDto
    {
        public virtual string ClientName { get; set; }
        public virtual string pan_no { get; set; }
    }

    
    public class EditclientDto:EntityDto
    {
        public virtual string ClientName { get; set; }
        public virtual string pan_no { get; set; }
    }
}
