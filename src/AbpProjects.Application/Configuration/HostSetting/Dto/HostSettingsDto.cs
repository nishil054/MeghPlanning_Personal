using AbpProjects.HostSetting.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration.HostSetting.Dto
{
   public class HostSettingsDto
    {
        
        public GeneralSettingsDto General { get; set; }

    
        public HostUserManagementSettingsDto UserManagement { get; set; }

        public EmailSettingsDto Email { get; set; }

      
        public TenantManagementSettingsDto TenantManagement { get; set; }

        
        public SecuritySettingsDto Security { get; set; }

        public AbpSettingsDto CDNDoc { get; set; }
    }
}
