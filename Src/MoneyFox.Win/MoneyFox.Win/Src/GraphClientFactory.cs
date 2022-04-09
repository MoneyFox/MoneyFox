namespace MoneyFox.Win;

using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using MoneyFox.Infrastructure.DbBackup;

public class GraphClientFactory : IGraphClientFactory
{
    public GraphServiceClient CreateClient(AuthenticationResult authResult)
    {
        return new(
            new DelegateAuthenticationProvider(
                requestMessage =>
                {
                    requestMessage.Headers.Authorization = new(scheme: "bearer", parameter: authResult.AccessToken);

                    return Task.CompletedTask;
                }));
    }
}
