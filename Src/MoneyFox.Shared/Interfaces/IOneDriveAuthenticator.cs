using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;

namespace MoneyFox.Shared.Interfaces
{
    public interface IOneDriveAuthenticator
    {
        Task<IOneDriveClient> LoginAsync();
    
        Task LogoutAsync();
    }
}