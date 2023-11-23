using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration.HostSetting.Dto
{
    public class AbpSettingsDto : EntityDto<long>
    {
        public virtual int? TenantId { get; set; }
        public virtual long? UserId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
        public string CDNUrl { get; set; }
        public string CDNFolderName { get; set; }
        public string CDNUserName { get; set; }
        public string CDNKey { get; set; }
        public string CDNContainer { get; set; }
        public bool CDNFlag { get; set; }
    }

}
