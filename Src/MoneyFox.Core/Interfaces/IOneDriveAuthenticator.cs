using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;

namespace MoneyFox.Core.Interfaces
{
    public interface IOneDriveAuthenticator
    {
        Task<IOneDriveClient> LoginAsync();
    }
}