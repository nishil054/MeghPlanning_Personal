using AbpProjects.Permissions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public string Rolename { get; set; }
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}
