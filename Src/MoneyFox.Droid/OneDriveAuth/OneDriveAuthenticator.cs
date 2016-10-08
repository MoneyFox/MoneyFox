using System;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Exceptions;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Droid.OneDriveAuth
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator {
        public async Task<IOneDriveClient> LoginAsync() {
            try {
                return new OneDriveClient(ServiceConstants.BASE_URL, new DroidAuthenticationProvider());
            } catch (Exception) {
                throw new BackupException("Authentication Failed");
            }
        }
    }
}