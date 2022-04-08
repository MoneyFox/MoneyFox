namespace MoneyFox.Infrastructure.DbBackup
{
    using Microsoft.Graph;
    using System.Threading;
    using System.Threading.Tasks;

    internal interface IOneDriveAuthenticationService
    {
        Task<GraphServiceClient> CreateServiceClient(CancellationToken cancellationToken = default);
        Task LogoutAsync(CancellationToken cancellationToken = default);
    }
}