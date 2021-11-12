using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace MoneyFox.Infrastructure.DbBackup
{
    public interface IGraphClientFactory
    {
        GraphServiceClient CreateClient(AuthenticationResult authResult);
    }
}