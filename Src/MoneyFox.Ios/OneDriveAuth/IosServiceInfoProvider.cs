using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;

namespace MoneyFox.Ios.OneDriveAuth {
    public class IosServiceInfoProvider : ServiceInfoProvider {
        public override async Task<ServiceInfo> GetServiceInfo(
            AppConfig appConfig,
            CredentialCache credentialCache,
            IHttpProvider httpProvider,
            ClientType clientType) {
            var serviceInfo = await base.GetServiceInfo(appConfig, credentialCache, httpProvider, clientType);

            var authProvider = new IosAuthenticationProvider(serviceInfo);
            serviceInfo.AuthenticationProvider = authProvider;

            return serviceInfo;
        }
    }
}