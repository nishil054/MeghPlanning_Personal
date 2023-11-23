using AbpProjects.Permissions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Permissions
{
    public class ListResultDto<T>
    {
        private List<FlatPermissionWithLevelDto> lists;

        public ListResultDto(List<FlatPermissionWithLevelDto> lists)
        {
            this.lists = lists;
        }
    }
}
