using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace MoneyFox.Application.Common.CloudBackup
{
    public interface IGraphClientFactory
    {
        GraphServiceClient CreateClient(AuthenticationResult authResult);
    }
}
