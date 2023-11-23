using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Users.Dto
{
    public class UpdateUserPermissionsInput
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public List<string> GrantedPermissionNames { get; set; }
    }
}
