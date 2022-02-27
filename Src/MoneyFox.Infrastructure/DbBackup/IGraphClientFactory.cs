namespace MoneyFox.Infrastructure.DbBackup
{
    using Microsoft.Graph;
    using Microsoft.Identity.Client;

    // TODO move this to core after isolation GraphServiceClient better
    public interface IGraphClientFactory
    {
        GraphServiceClient CreateClient(AuthenticationResult authResult);
    }
}