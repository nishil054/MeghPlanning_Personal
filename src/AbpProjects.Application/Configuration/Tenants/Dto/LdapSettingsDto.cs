using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration.Tenants.Dto
{
   public class LdapSettingsDto
    {
        public bool IsModuleEnabled { get; set; }

        public bool IsEnabled { get; set; }

        public string Domain { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
