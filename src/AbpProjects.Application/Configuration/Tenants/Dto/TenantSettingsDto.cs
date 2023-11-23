using Abp.Runtime.Validation;
using AbpProjects.Configuration.HostSetting.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration.Tenants.Dto
{
    public class TenantSettingsDto
    {
        public GeneralSettingsDto General { get; set; }

        [Required]
        public TenantUserManagementSettingsDto UserManagement { get; set; }

        public EmailSettingsDto Email { get; set; }

        public LdapSettingsDto Ldap { get; set; }

        [Required]
        public SecuritySettingsDto Security { get; set; }

        /// <summary>
        /// This validation is done for single-tenant applications.
        /// Because, these settings can only be set by tenant in a single-tenant application.
        /// </summary>
        public void ValidateHostSettings()
        {
            var validationErrors = new List<ValidationResult>();
            if (General == null)
            {
                validationErrors.Add(new ValidationResult("General settings can not be null", new[] { "General" }));
            }

            if (Email == null)
            {
                validationErrors.Add(new ValidationResult("Email settings can not be null", new[] { "Email" }));
            }

            if (validationErrors.Count > 0)
            {
                throw new AbpValidationException("Method arguments are not valid! See ValidationErrors for details.", validationErrors);
            }
        }
    }
}
