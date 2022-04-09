namespace MoneyFox.iOS
{

    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Infrastructure.DbBackup;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;

    public class GraphServiceClientFactory : IGraphClientFactory
    {
        private static NSUrlSessionHandler HttpMessageHandler => new NSUrlSessionHandler { BypassBackgroundSessionCheck = true };

        public GraphServiceClient CreateClient(AuthenticationResult authResult)
        {
            var authProvider = new DelegateAuthenticationProvider(
                reqMsg =>
                {
                    reqMsg.Headers.Authorization = new AuthenticationHeaderValue(scheme: "bearer", parameter: authResult.AccessToken);

                    return Task.CompletedTask;
                });

            var pipeline = GraphClientFactory.CreatePipeline(
                handlers: GraphClientFactory.CreateDefaultHandlers(authProvider),
                finalHandler: HttpMessageHandler);

            var httpProvider = new HttpProvider(httpMessageHandler: pipeline, disposeHandler: true, serializer: new Serializer());

            return new GraphServiceClient(authenticationProvider: authProvider, httpProvider: httpProvider);
        }
    }

}
