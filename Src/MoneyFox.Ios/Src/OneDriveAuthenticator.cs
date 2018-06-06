using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.iOS.Src
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        public Task<IOneDriveClient> LoginAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
