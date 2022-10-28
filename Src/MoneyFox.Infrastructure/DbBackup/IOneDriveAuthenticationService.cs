namespace MoneyFox.Infrastructure.DbBackup;

using System.Threading;
using System.Threading.Tasks;
using OneDriveModels;

public interface IOneDriveAuthenticationService
{
    Task<OneDriveAuthentication> AcquireAuthentication(CancellationToken cancellationToken = default);

    Task LogoutAsync(CancellationToken cancellationToken = default);
}
