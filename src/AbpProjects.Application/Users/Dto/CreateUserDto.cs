using System;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using AbpProjects.Authorization.Users;

namespace AbpProjects.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class CreateUserDto
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string[] RoleNames { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public DateTime? Date { get; set; }
        public int CompanyId { get; set; }

        [Required]
        public decimal Salary_Hour { get; set; }

        [Required]
        public decimal Salary_Month { get; set; }


        [Required(ErrorMessage = "Please Enter Birthdate")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Please Enter Next_Renewaldate")]
        public DateTime Next_Renewaldate { get; set; }

        [Required(ErrorMessage = "Please Enter JoiningDate")]
        public DateTime Joiningdate { get; set; }

        public DateTime? Resigndate { get; set; }

        public DateTime? Lastdate { get; set; }


        public int? Immediate_supervisorId { get; set; }

        public int? TeamId { get; set; }
        public decimal LeaveBalance { get; set; }
        public virtual decimal? TargetAmount { get; set; }
    }
}