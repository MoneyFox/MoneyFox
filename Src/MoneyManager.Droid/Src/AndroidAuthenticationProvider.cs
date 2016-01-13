using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using Cirrious.CrossCore;

namespace MoneyManager.Droid
{
    public class AndroidAuthenticationProvider : AuthenticationProvider
    {
        public AndroidAuthenticationProvider(ServiceInfo serviceInfo) : base(serviceInfo)
        {
        }

        protected override async Task<AccountSession> GetAuthenticationResultAsync()
        {
            return new AccountSession(Mvx.Resolve<TempMessage>().AuthenticationResponseValues, this.ServiceInfo.AppId, AccountType.MicrosoftAccount)
            {
                CanSignOut = true
            };
        }

        public override Task SignOutAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}