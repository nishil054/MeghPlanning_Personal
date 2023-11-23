using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using AbpProjects.Authorization.Users;

namespace AbpProjects.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
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

        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public string[] Roles { get; set; }
        public int? CompanyId { get; set; }

        [Required]
        public decimal Salary_Hour { get; set; }

        [Required]
        public decimal Salary_Month { get; set; }


        [Required(ErrorMessage = "Please Enter Birthdate")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Please Enter Next_Renewaldate")]
        public DateTime? Next_Renewaldate { get; set; }

        [Required(ErrorMessage = "Please Enter JoiningDate")]
        public DateTime Joiningdate { get; set; }
        public DateTime? Resigndate { get; set; }

        public DateTime? Lastdate { get; set; }


        public int? Immediate_supervisorId { get; set; }

        public int? TeamId { get; set; }

        public virtual string TeamName { get; set; }
        public virtual string CompanyName { get; set; }
        public decimal LeaveBalance { get; set; }
        public int RoleId { get; set; }
        public virtual string RoleName { get; set; }
        public virtual List<string> Role_Name { get; set; }
        public virtual decimal? TargetAmount { get; set; }
        public virtual decimal PendingLeaves { get; set; }
    }

    public class RoleNameDisplay
    {
        public virtual string Name { get; set; }
    }
}
