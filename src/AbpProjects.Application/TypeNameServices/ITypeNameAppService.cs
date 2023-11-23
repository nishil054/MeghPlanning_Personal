using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.TypenameServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.TypenameServices
{
 public  interface ITypenameAppService : IApplicationService
    {
        Task CreateTypename(CreateTypenameDto input);
        bool TypenameExsistence(TypenameDto input);
        bool TypenameExsistenceById(TypenameDto input);
        List<TypenameDto> GetTypename();
        Task<TypenameDto> GetDataById(EntityDto input);
        Task UpdateTypename(EditTypenameDto input);
        Task DeleteTypename(EntityDto input);
        PagedResultDto<TypenameDto> GetTypenameList(GetTypenameDto input);
    }
}
