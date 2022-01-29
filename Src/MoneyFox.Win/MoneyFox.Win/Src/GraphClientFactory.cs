using Microsoft.Graph;
using Microsoft.Identity.Client;
using MoneyFox.Infrastructure.DbBackup;
using System.Net.Http.Headers;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Win
{
    public class GraphClientFactory : IGraphClientFactory
    {
        public GraphServiceClient CreateClient(AuthenticationResult authResult) =>
            new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    requestMessage =>
                    {
                        requestMessage.Headers.Authorization =
                            new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                        return Task.CompletedTask;
                    }));
    }
}