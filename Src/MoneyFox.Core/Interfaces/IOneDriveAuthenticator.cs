using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;

namespace MoneyFox.Foundation.Interfaces
{
    public interface IOneDriveAuthenticator
    {
        Task<IOneDriveClient> LoginAsync();
    }
}