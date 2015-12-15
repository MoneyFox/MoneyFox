using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IOneDriveAuthenticator
    {
        Task<IOneDriveClient> LoginAsync();
    }
}
