using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Clientsaddress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Clients.Dto
{
    [AutoMapTo(typeof(ClientAddress))]
    public class ClientdetailDto:EntityDto
    {
        //public virtual string ClientName { get; set; }
        //public virtual string pan_no { get; set; }
        public virtual int? clientid { get; set; }
        public virtual string clientaddress { get; set; }

        public virtual string city { get; set; }
       
        public virtual string state { get; set; }
      
        public virtual string pincode { get; set; }
        public virtual string Contactname { get; set; }
      
        public virtual string Email { get; set; }
        
        public virtual string Contactno { get; set; }
        public virtual bool? isdefault { get; set; }
        public virtual int? statecodeid { get; set; }
        
        public virtual string gstno { get; set; }
        
        public virtual string CountryName { get; set; }
    }


    public class Clientdetail_Dto 
    {
        public virtual string ClientName { get; set; }
        public virtual string pan_no { get; set; }
        public virtual List<ClientdetailDto> ClientAdd { get; set; }
    }

    public class CheckExsistDto
    {
        public virtual int? Id { get; set; }
        public virtual string CName { get; set; }
    }
}
