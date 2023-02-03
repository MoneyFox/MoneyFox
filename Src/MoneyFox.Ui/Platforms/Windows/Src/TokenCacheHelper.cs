namespace MoneyFox.Ui.Platforms.Windows.Src;

using System.Security.Cryptography;
using global::Windows.Storage;
using Microsoft.Identity.Client;

internal static class TokenCacheHelper
{
    /// <summary>
    ///     Path to the token cache
    /// </summary>
    private static readonly string cacheFilePath = ApplicationData.Current.LocalFolder.Path + ".msalcache.bin";

    private static readonly object fileLock = new();

    private static void BeforeAccessNotification(TokenCacheNotificationArgs args)
    {
        lock (fileLock)
        {
            args.TokenCache.DeserializeMsalV3(
                File.Exists(cacheFilePath)
                    ? ProtectedData.Unprotect(encryptedData: File.ReadAllBytes(cacheFilePath), optionalEntropy: null, scope: DataProtectionScope.CurrentUser)
                    : null);
        }
    }

    private static void AfterAccessNotification(TokenCacheNotificationArgs args)
    {
        // if the access operation resulted in a cache update
        if (args.HasStateChanged)
        {
            lock (fileLock)
            {
                // reflect changes in the persistent store
                File.WriteAllBytes(
                    path: cacheFilePath,
                    bytes: ProtectedData.Protect(userData: args.TokenCache.SerializeMsalV3(), optionalEntropy: null, scope: DataProtectionScope.CurrentUser));
            }
        }
    }

    internal static void EnableSerialization(ITokenCache tokenCache)
    {
        tokenCache.SetBeforeAccess(BeforeAccessNotification);
        tokenCache.SetAfterAccess(AfterAccessNotification);
    }
}
