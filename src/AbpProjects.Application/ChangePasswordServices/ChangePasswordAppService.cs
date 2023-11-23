using Abp.Authorization;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.UI;
using AbpProjects.Authorization.Accounts.Dto;
using AbpProjects.Authorization.Users;
using AbpProjects.ChangePasswordServices.Dto;
using AbpProjects.Sessions;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ChangePasswordServices
{
    [AbpAuthorize]
    public class ChangePasswordAppService : AbpProjectsAppServiceBase, IChangePasswordAppService
    {
        private readonly UserManager _userManager;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _session;

        public ChangePasswordAppService(
            UserManager userManager,
            ISessionAppService sessionAppService,
            IAbpSession session
            )
        {
            _userManager = userManager;
            _sessionAppService = sessionAppService;
            _session = session;
        }
        public async Task<string> ChangePassword(ChangePasswordDto input)
        {
            try
            {
                if (input.UserId.HasValue)
                {
                    var user = await _userManager.GetUserByIdAsync(input.UserId.Value);
                    var correctpassword = _userManager.CheckPassword(user, input.OldPassword);

                    string Message = "";
                    if (correctpassword)
                    {
                        if (input.OldPassword == input.Password)
                        {
                            Message = "OldPasswordNewPasswordSame";
                        }
                        else if (input.Password != input.ConfirmPassword)
                        {
                            Message = "NewPasswordConfirmNewPasswordNotMatch";
                        }
                        else
                        {
                            Message = "Correct";
                        }
                    }
                    else
                    {
                        Message = "OldPasswordIncorrect";
                    }
                    if (!string.IsNullOrEmpty(Message) && Message == "Correct")
                    {
                        if (!input.Password.IsNullOrEmpty())
                        {
                            CheckErrors(await _userManager.ChangePasswordAsync(user, input.Password));
                        }
                    }

                    await _userManager.UpdateAsync(user);
                    return Message;
                }
                else
                {
                    var loginInformations = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations());
                    var user = await _userManager.GetUserByIdAsync(loginInformations.User.Id);
                    var correctpassword = _userManager.CheckPassword(user, input.OldPassword);

                    string Message = "";
                    if (correctpassword)
                    {
                        if (input.OldPassword == input.Password)
                        {
                            Message = "OldPasswordNewPasswordSame";
                        }
                        else if (input.Password != input.ConfirmPassword)
                        {
                            Message = "NewPasswordConfirmNewPasswordNotMatch";
                        }
                        else
                        {
                            Message = "Correct";
                        }
                    }
                    else
                    {
                        Message = "OldPasswordIncorrect";
                    }
                    if (!string.IsNullOrEmpty(Message) && Message == "Correct")
                    {
                        if (!input.Password.IsNullOrEmpty())
                        {
                            CheckErrors(await _userManager.ChangePasswordAsync(user, input.Password));
                        }
                    }

                    await _userManager.UpdateAsync(user);
                    return Message;
                }
                
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException();
            }
        }
    }
}
