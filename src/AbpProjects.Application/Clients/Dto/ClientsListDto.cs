using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Clientsaddress;
using AbpProjects.MeghPlanningSupports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Clients.Dto
{

    [AutoMapFrom(typeof(Client))]
   public class ClientsListDto : EntityDto
    {
       
        public virtual string ClientName { get; set; }
        public int? createdby { get; set; }
        public int? isadmin { get; set; }
        public bool? isDeleteEnable { get; set; }
    }
}
