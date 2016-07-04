using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Droid.OneDriveAuth;

namespace MoneyFox.Droid {
    public class DroidServiceInfoProvider : ServiceInfoProvider {
        public override async Task<ServiceInfo> GetServiceInfo(
            AppConfig appConfig,
            CredentialCache credentialCache,
            IHttpProvider httpProvider,
            ClientType clientType) {
            var serviceInfo = await base.GetServiceInfo(appConfig, credentialCache, httpProvider, clientType);

            var authProvider = new DroidAuthenticationProvider(serviceInfo);
            serviceInfo.AuthenticationProvider = authProvider;

            return serviceInfo;
        }
    }
}