using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Clients.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Clients
{
  public interface IClientsAppService : IApplicationService
    {
         Task<PagedResultDto<ClientsListDto>> GetClientsList(ClientsInput input);
        Task CreateClient(Clientdetail_Dto input);
        bool checkClient(int id);
        bool checkProject(int id);
        ListResultDto<EditAddressDto> GetClientdetailList(int id);
        Task Editclient(Clientdetail_Dto input, int id);
        //Task Editclient(List<ClientdetailDto> input, int id, string clientname, string pan_no);
        EditclientDto getClientdetail(int id);
        Task DeleteClients(int id);
        bool clientExsistence(CheckExsistDto input);
    }
}
