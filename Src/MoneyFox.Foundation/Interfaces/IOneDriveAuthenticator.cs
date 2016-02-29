using System.Threading.Tasks;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IOneDriveAuthenticator
    {
        Task<IOneDriveClient> LoginAsync();
    }
}