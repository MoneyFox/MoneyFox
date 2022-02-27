namespace MoneyFox.iOS
{
    using Infrastructure.DbBackup;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class GraphServiceClientFactory : IGraphClientFactory
    {
        private static NSUrlSessionHandler HttpMessageHandler =>
            new NSUrlSessionHandler {BypassBackgroundSessionCheck = true};

        public GraphServiceClient CreateClient(AuthenticationResult authResult)
        {
            var authProvider = new DelegateAuthenticationProvider(
                reqMsg =>
                {
                    reqMsg.Headers.Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                    return Task.CompletedTask;
                });

            HttpMessageHandler pipeline =
                GraphClientFactory.CreatePipeline(
                    GraphClientFactory.CreateDefaultHandlers(authProvider),
                    HttpMessageHandler);
            var httpProvider = new HttpProvider(pipeline, true, new Serializer());

            return new GraphServiceClient(authProvider, httpProvider);
        }
    }
}