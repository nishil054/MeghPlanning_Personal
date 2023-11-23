using Abp.Authorization;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;

namespace AbpProjects.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
