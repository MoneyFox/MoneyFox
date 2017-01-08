using System;
using System.Threading.Tasks;
using Windows.Security.Credentials;



namespace MoneyFox.Windows.Services
{
    class MicrosoftPassportHelper
    {
        public static async Task<bool> CreatePassportKeyAsync()
        {
            KeyCredentialRetrievalResult keyCreationResult = await KeyCredentialManager.RequestCreateAsync("temp", KeyCredentialCreationOption.ReplaceExisting);

            if (keyCreationResult.Status == KeyCredentialStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
