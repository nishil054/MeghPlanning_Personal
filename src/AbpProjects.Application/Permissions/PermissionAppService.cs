using Abp.Authorization;
using Abp.AutoMapper;
using AbpProjects.Permissions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;


namespace AbpProjects.Permissions
{
    public class PermissionAppService : AbpProjectsAppServiceBase, IPermissionAppService
    {
        public ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();
            var rootPermissions = permissions.Where(p => p.Parent == null);

            var result = new List<FlatPermissionWithLevelDto>();

            foreach (var rootPermission in rootPermissions)
            {
                var level = 0;
                AddPermission(rootPermission, permissions, result, level);
            }

            return new ListResultDto<FlatPermissionWithLevelDto>(result.MapTo<List<FlatPermissionWithLevelDto>>());

        }

        private void AddPermission(Permission permission, IReadOnlyList<Permission> allPermissions, List<FlatPermissionWithLevelDto> result, int level)
        {
            var flatPermission = permission.MapTo<FlatPermissionWithLevelDto>();
            flatPermission.Level = level;
            result.Add(flatPermission);

            if (permission.Children == null)
            {
                return;
            }

            var children = allPermissions.Where(p => p.Parent != null && p.Parent.Name == permission.Name).ToList();

            foreach (var childPermission in children)
            {
                AddPermission(childPermission, allPermissions, result, level + 1);
            }
        }
    }
}
