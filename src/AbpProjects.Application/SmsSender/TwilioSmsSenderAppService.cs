using Abp.Dependency;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace AbpProjects.SmsSender
{
    public class TwilioSmsSenderAppService : ISmsSenderAppService, ITransientDependency
    {
        private TwilioSmsSenderConfigurationAppService _twilioSmsSenderConfiguration;
        public TwilioSmsSenderAppService(TwilioSmsSenderConfigurationAppService twilioSmsSenderConfiguration)
        {
            _twilioSmsSenderConfiguration = twilioSmsSenderConfiguration;
        }
        public async Task SendAsync(string number, string message)
        {
            TwilioClient.Init(_twilioSmsSenderConfiguration.AccountSid, _twilioSmsSenderConfiguration.AuthToken);
            MessageResource resource = await MessageResource.CreateAsync(
                body: message,
                @from: new Twilio.Types.PhoneNumber(_twilioSmsSenderConfiguration.SenderNumber),
                to: new Twilio.Types.PhoneNumber(number)
            );
        }
    }
}
