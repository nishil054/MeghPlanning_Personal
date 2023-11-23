using Abp.Auditing;
using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ChangePasswordServices.Dto
{
    public class ChangePasswordDto
    {
        public int? UserId { get; set; }
        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string ConfirmPassword { get; set; }
    }
}
