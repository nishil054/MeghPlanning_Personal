using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.Clients.Dto;
using AbpProjects.Clientsaddress;
using AbpProjects.MeghPlanningSupports;
using AbpProjects.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace AbpProjects.Clients
{
    [AbpAuthorize(PermissionNames.Pages_Clients)]
    public class ClientsAppService : AbpProjectsApplicationModule, IClientsAppService
    {
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<ClientAddress> _clientAddressRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<project> _projectRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly UserManager _userManager;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        public ClientsAppService(IRepository<Client> clientRepository, IRepository<ClientAddress> clientAddressRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<project> projectRepository, IAbpSession abpSession, IRepository<User, long> userRepository, IRepository<Role> roleRepository, UserManager userManager, IRepository<UserRole, long> userRoleRepository)
        {

            _clientRepository = clientRepository;
            _clientAddressRepository = clientAddressRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _projectRepository = projectRepository;
            _abpSession = abpSession;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _userRoleRepository = userRoleRepository;

        }
        public async Task<PagedResultDto<ClientsListDto>> GetClientsList(ClientsInput input)
        {
            int createduser = (int)_abpSession.UserId;

            var role = (from user in _userRepository.GetAll()
                        join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                        from ur in urJoined.DefaultIfEmpty()
                        join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                        from us in usJoined.DefaultIfEmpty()
                        where (us != null && user.Id == _abpSession.UserId)
                        select (us.DisplayName)).ToList();
            bool rolename = false;
            if (role.Contains("Admin")) { rolename = true; }
            var Client = (from item in _clientRepository
             .GetAll()
             .WhereIf(!input.search.IsNullOrEmpty(), s => s.ClientName.ToLower().Contains(input.search.ToLower()))
                          select new ClientsListDto
                          {
                              Id = item.Id,
                              ClientName = item.ClientName,
                              createdby = item.CreatorUserId == createduser ? 1 : 0,
                              isadmin = rolename == true ? 1 : 0,
                              isDeleteEnable = false
                          });

            var entites = Client.OrderBy(input.Sorting).PageBy(input).ToList();
            if (entites.Count > 0)
            {
                foreach (var item in entites)
                {
                    item.isDeleteEnable = _projectRepository.GetAll()
                         .Where(e => e.ClientId == item.Id)
                         .Any();
                }
            }
            //var entitiesCount = data.Count();

            var clientcount = _clientRepository.GetAll()
                .WhereIf(!input.search.IsNullOrEmpty(), s => s.ClientName.ToLower().Contains(input.search.ToLower())).Count();


            return new PagedResultDto<ClientsListDto>(clientcount, entites.MapTo<List<ClientsListDto>>());

        }
        public async Task CreateClient(Clientdetail_Dto input)
        {
            try
            {

                var id = 0;
                Client obj = new Client();
                obj.ClientName = input.ClientName;
                obj.pan_no = input.pan_no;

                //  id = await _clientRepository.InsertAndGetIdAsync(obj);
                //obj.clientid
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    await _clientRepository.InsertAndGetIdAsync(obj);

                    id = obj.Id;
                    unitOfWork.Complete();
                }

                var dt = _clientRepository.GetAll().Where(p => p.Id == id).FirstOrDefault();
                if (dt != null)
                {
                    dt.Clientid = id;
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        _clientRepository.UpdateAsync(dt);
                        unitOfWork.Complete();
                    }
                }

              
                List<ClientAddress> clientsavedata = new List<ClientAddress>();

                clientsavedata = input.ClientAdd.MapTo<List<ClientAddress>>();
                foreach (var item in clientsavedata)
                {
                    item.clientid = id;
                    var clientid = await _clientAddressRepository.InsertAndGetIdAsync(item);
                }

            }
            catch (Exception ex) { throw ex; }
        }

        public bool checkClient(int id)
        {
            bool existdata = (from act in _clientAddressRepository.GetAll().Where(r => r.clientid == id)
                              select act.Id).Any();
            return existdata;
        }
        public bool clientExsistence(CheckExsistDto input)
        {
            var items = _clientRepository.GetAll()
                        .Where(e => e.ClientName == input.CName)
                        .WhereIf(input.Id.HasValue, x => x.Id != input.Id)
                        .Any();
            return items;
        }

        public bool checkProject(int id)
        {
            bool existdata = (from act in _projectRepository.GetAll().Where(r => r.ClientId == id)
                              select act.Id).Any();
            return existdata;
        }
        public ListResultDto<EditAddressDto> GetClientdetailList(int id)
        {
            //var client = _clientAddressRepository.GetAll().Where(p=>p.clientid==id).OrderBy(p => p.Id).ToList();
            var client = (from i in _clientAddressRepository.GetAll()
                          where (i.clientid == id)
                          select new EditAddressDto
                          {
                              Id = i.Id,
                              clientid = i.clientid,
                              clientaddress = i.clientaddress,
                              city = i.city,
                              state = i.city,
                              Contactname = i.Contactname,
                              Email = i.Email,
                              Contactno = i.Contactno,
                              isdefault = i.isdefault,
                              statecodeid = i.statecodeid,
                              gstno = i.gstno,
                              CountryName = i.CountryName,
                              pincode = i.pincode

                          }).ToList();
            return new ListResultDto<EditAddressDto>(client.MapTo<List<EditAddressDto>>());
        }
        public EditclientDto getClientdetail(int id)
        {
            var client = (from i in _clientRepository.GetAll()
                          where (i.Id == id)
                          select new EditclientDto { Id = i.Id, ClientName = i.ClientName, pan_no = i.pan_no }).FirstOrDefault();
            return client;
        }

        public async Task Editclient(Clientdetail_Dto input, int id)
        {
            try
            {
                var clientt = (from c in _clientRepository.GetAll()
                               where c.Id == id
                               select new InputData
                               {
                                   Id = c.Id,
                                   ClientName = c.ClientName,
                                   PanNo = c.pan_no
                               }).FirstOrDefault();

                Client h = new Client();
                h.Id = id;
                h.ClientName = input.ClientName;
                h.pan_no = input.pan_no;
                //h.Id = clientt.Id;

                _clientRepository.Update(h);

                // var linkdata = _clientAddressRepository.GetAll().Where(p => p.clientid == id).OrderBy(p => p.Id).ToList();
                var dataAdd = (from i in _clientAddressRepository.GetAll()
                               where i.clientid == id
                               select new DataAdd
                               {
                                   Id = i.Id
                               }).ToList();
                foreach (var item in dataAdd)
                {
                    await _clientAddressRepository.DeleteAsync(item.Id);
                }

                List<ClientAddress> clientsavedata = new List<ClientAddress>();

                clientsavedata = input.ClientAdd.MapTo<List<ClientAddress>>();
                foreach (var item in clientsavedata)
                {
                    item.clientid = id;
                    var clientid = await _clientAddressRepository.InsertAndGetIdAsync(item);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteClients(int id)
        {
            try
            {
                var clientadress = _clientAddressRepository.GetAll().Where(p => p.clientid == id).ToList();
                foreach (var item in clientadress)
                {

                    await _clientAddressRepository.DeleteAsync(item);
                }
                var client = _clientRepository.GetAll().Where(p => p.Id == id).ToList();
                foreach (var item in client)
                {

                    await _clientRepository.DeleteAsync(item);
                }
                // await _clientRepository.DeleteAsync(id);
            }
            catch (Exception e) { throw e; }
        }
    }

    internal class DataAdd
    {
        public int Id { get; set; }
    }
}
