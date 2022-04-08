namespace MoneyFox.Infrastructure.DbBackup
{

    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    
    public interface IGraphClientFactory
    {
        GraphServiceClient CreateClient(AuthenticationResult authResult);
    }

}
