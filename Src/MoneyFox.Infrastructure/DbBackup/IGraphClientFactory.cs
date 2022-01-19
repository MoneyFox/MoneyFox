using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace MoneyFox.Infrastructure.DbBackup
{
    // TODO move this to core after isolation GraphServiceClient better
    public interface IGraphClientFactory
    {
        GraphServiceClient CreateClient(AuthenticationResult authResult);
    }
}