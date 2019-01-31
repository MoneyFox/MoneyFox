using System;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace MoneyFox.Uwp
{
    public class MicrosoftPassportHelper
    {
        public static async Task<bool> TestPassportAvailable()
        {
            return await KeyCredentialManager.IsSupportedAsync();
        }

        public static async Task<bool> CreatePassportKeyAsync()
        {
            var keyCreationResult = await KeyCredentialManager.RequestCreateAsync("temp", KeyCredentialCreationOption.ReplaceExisting);

            return keyCreationResult.Status == KeyCredentialStatus.Success;
        }
    }
}
