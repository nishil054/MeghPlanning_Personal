using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using Microsoft.AspNet.Identity;

namespace AbpProjects.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public User()
        {
           
        }
        public virtual int? CompanyId { get; set; }

        public virtual decimal Salary_Hour { get; set; }
        public virtual decimal Salary_Month { get; set; }

        public virtual DateTime? Birthdate { get; set; }

        public virtual DateTime? Next_Renewaldate { get; set; }

        public virtual DateTime? Joiningdate { get; set; }
        public virtual DateTime? Resigndate { get; set; }
        public virtual DateTime? Lastdate { get; set; }


        public virtual int? Immediate_supervisorId { get; set; }

        public virtual int TeamId { get; set; }
        public virtual decimal LeaveBalance { get; set; }
        public virtual decimal? TargetAmount { get; set; }
        
        public virtual DateTime? LeaveUpdateDate { get; set; }
        public virtual decimal PendingLeaves { get; set; }
        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string password)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Password = new PasswordHasher().HashPassword(password)
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}