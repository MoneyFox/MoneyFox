namespace MoneyFox.Droid
{

    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Infrastructure.DbBackup;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;

    public class GraphClientFactory : IGraphClientFactory
    {
        public GraphServiceClient CreateClient(AuthenticationResult authResult)
        {
            return new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    requestMessage =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(scheme: "bearer", parameter: authResult.AccessToken);

                        return Task.CompletedTask;
                    }));
        }
    }

}
