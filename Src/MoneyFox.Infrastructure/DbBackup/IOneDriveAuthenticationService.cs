namespace MoneyFox.Infrastructure.DbBackup
{

    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Graph;
    using MoneyFox.Infrastructure.DbBackup.OneDriveModels;

    internal interface IOneDriveAuthenticationService
    {
        Task<GraphServiceClient> CreateServiceClient(CancellationToken cancellationToken = default);

        Task<OneDriveAuthentication> AcquireAuthentication(CancellationToken cancellationToken = default);

        Task LogoutAsync(CancellationToken cancellationToken = default);
    }

}
