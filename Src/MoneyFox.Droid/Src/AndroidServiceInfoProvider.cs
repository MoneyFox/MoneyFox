using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;

namespace MoneyFox.Droid {
    public class AndroidServiceInfoProvider : ServiceInfoProvider {
        public override async Task<ServiceInfo> GetServiceInfo(
            AppConfig appConfig,
            CredentialCache credentialCache,
            IHttpProvider httpProvider,
            ClientType clientType) {
            var serviceInfo = await base.GetServiceInfo(appConfig, credentialCache, httpProvider, clientType);

            var authProvider = new AndroidAuthenticationProvider(serviceInfo);
            serviceInfo.AuthenticationProvider = authProvider;

            return serviceInfo;
        }
    }
}