using Microsoft.Graph;
using Microsoft.Identity.Client;
using MoneyFox.Application.Common.CloudBackup;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.Src
{
    public class GraphClientFactory : IGraphClientFactory
    {
        public GraphServiceClient CreateClient(AuthenticationResult authResult)
        {
            return new GraphServiceClient(new DelegateAuthenticationProvider(requestMessage =>
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                return Task.CompletedTask;
            }));
        }
    }
}
