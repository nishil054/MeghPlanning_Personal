using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Clientsaddress
{
    [Table("tbl_ClientAddress")]
    public  class ClientAddress : AuditedEntity
    {
        public const int maxLengthclientaddress = 200;
        public const int maxLengthcity = 50;
        public const int maxstate = 50;
        public const int maxEmail = 50;
        public const int maxpincode = 7;
        public const int maxContactname = 100;
        public const int maxContactno = 10;
        public const int maxgstno = 100;
        public const int maxCountryName = 200;
        public virtual int? clientid { get; set; }
         
        [MaxLength(maxLengthclientaddress)]
        public virtual string clientaddress { get; set; }
        
        [MaxLength(maxLengthcity)]
        public virtual string city  { get; set; }
        [MaxLength(maxstate)]
        public virtual string state   { get; set; }
        [MaxLength(maxpincode)]
        public virtual string pincode   { get; set; }
        [MaxLength(maxContactname)]
        public virtual string Contactname  { get; set; }
        [MaxLength(maxEmail)]
        public virtual string Email   { get; set; }
        [MaxLength(maxContactno)] 
        public virtual string Contactno  { get; set; }
        public virtual bool?  isdefault { get; set; }
        public virtual int? statecodeid   { get; set; }
        [MaxLength(maxgstno)]
        public virtual string gstno      { get; set; }
        [MaxLength(maxCountryName)]
        public virtual string CountryName      { get; set; }
       
    }
}
